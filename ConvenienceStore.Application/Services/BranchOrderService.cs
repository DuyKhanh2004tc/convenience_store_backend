using ConvenienceStore.Application.Contracts.Persistence;
using ConvenienceStore.Application.DTOs.BranchOrder;
using ConvenienceStore.Application.Services.Interfaces;
using ConvenienceStore.Domain.Entities;
using ConvenienceStore.Domain.Enums;

namespace ConvenienceStore.Application.Services;

public class BranchOrderService : IBranchOrderService
{
    private readonly IGenericRepository<BranchOrder> _orderRepository;
    private readonly IGenericRepository<BranchOrderDetail> _orderDetailRepository;
    private readonly IGenericRepository<WarehouseInventory> _warehouseInventoryRepository;
    private readonly IGenericRepository<BranchInventory> _branchInventoryRepository;
    private readonly IGenericRepository<Product> _productRepository;
    private readonly IGenericRepository<Branch> _branchRepository;
    private readonly IGenericRepository<User> _userRepository;
    private readonly IUnitOfWork _unitOfWork;

    public BranchOrderService(
        IGenericRepository<BranchOrder> orderRepository,
        IGenericRepository<BranchOrderDetail> orderDetailRepository,
        IGenericRepository<WarehouseInventory> warehouseInventoryRepository,
        IGenericRepository<BranchInventory> branchInventoryRepository,
        IGenericRepository<Product> productRepository,
        IGenericRepository<Branch> branchRepository,
        IGenericRepository<User> userRepository,
        IUnitOfWork unitOfWork)
    {
        _orderRepository = orderRepository;
        _orderDetailRepository = orderDetailRepository;
        _warehouseInventoryRepository = warehouseInventoryRepository;
        _branchInventoryRepository = branchInventoryRepository;
        _productRepository = productRepository;
        _branchRepository = branchRepository;
        _userRepository = userRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<BranchOrderDto> CreateOrderAsync(int createdByUserId, CreateBranchOrderDto request)
    {
        // 1. Validate Branch exists
        var branch = await _branchRepository.GetByIdAsync(request.BranchId);
        if (branch == null)
            throw new Exception("Branch not found.");

        // 2. Validate all products exist
        foreach (var item in request.Items)
        {
            var product = await _productRepository.GetByIdAsync(item.ProductId);
            if (product == null)
                throw new Exception($"Product with ID {item.ProductId} not found.");
            if (!product.IsActive)
                throw new Exception($"Product '{product.ProductName}' is currently inactive.");
        }

        // 3. Check for duplicate product IDs in the request
        var duplicateProducts = request.Items
            .GroupBy(i => i.ProductId)
            .Where(g => g.Count() > 1)
            .Select(g => g.Key);
        if (duplicateProducts.Any())
            throw new Exception($"Duplicate product IDs found: {string.Join(", ", duplicateProducts)}");

        // 4. Create the order
        var order = new BranchOrder
        {
            BranchId = request.BranchId,
            CreatedBy = createdByUserId,
            OrderDate = DateTime.UtcNow,
            Status = OrderStatus.Pending,
            UpdatedAt = DateTime.UtcNow
        };

        await _orderRepository.AddAsync(order);
        await _unitOfWork.SaveChangesAsync(); // Save to get the generated OrderId

        // 5. Create order details
        var orderDetails = request.Items.Select(item => new BranchOrderDetail
        {
            OrderId = order.OrderId,
            ProductId = item.ProductId,
            Quantity = item.Quantity
        }).ToList();

        await _orderDetailRepository.AddRangeAsync(orderDetails);
        await _unitOfWork.SaveChangesAsync();

        return await BuildOrderDto(order);
    }

    public async Task<IEnumerable<BranchOrderDto>> GetAllOrdersAsync(int? branchId = null, string? status = null)
    {
        IReadOnlyList<BranchOrder> orders;

        if (branchId.HasValue && !string.IsNullOrEmpty(status))
        {
            if (!Enum.TryParse<OrderStatus>(status, true, out var parsedStatus))
                throw new Exception($"Invalid status value: {status}");

            orders = await _orderRepository.GetAsync(o => o.BranchId == branchId.Value && o.Status == parsedStatus);
        }
        else if (branchId.HasValue)
        {
            orders = await _orderRepository.GetAsync(o => o.BranchId == branchId.Value);
        }
        else if (!string.IsNullOrEmpty(status))
        {
            if (!Enum.TryParse<OrderStatus>(status, true, out var parsedStatus))
                throw new Exception($"Invalid status value: {status}");

            orders = await _orderRepository.GetAsync(o => o.Status == parsedStatus);
        }
        else
        {
            orders = await _orderRepository.GetAllAsync();
        }

        var result = new List<BranchOrderDto>();
        foreach (var order in orders.OrderByDescending(o => o.OrderDate))
        {
            result.Add(await BuildOrderDto(order));
        }

        return result;
    }

    public async Task<BranchOrderDto?> GetOrderByIdAsync(int orderId)
    {
        var order = await _orderRepository.GetByIdAsync(orderId);
        if (order == null) return null;

        return await BuildOrderDto(order);
    }

    public async Task<IEnumerable<BranchOrderDto>> GetOrdersByBranchAsync(int branchId)
    {
        var orders = await _orderRepository.GetAsync(o => o.BranchId == branchId);

        var result = new List<BranchOrderDto>();
        foreach (var order in orders.OrderByDescending(o => o.OrderDate))
        {
            result.Add(await BuildOrderDto(order));
        }

        return result;
    }

    public async Task<BranchOrderDto> ApproveOrderAsync(int orderId)
    {
        var order = await _orderRepository.GetByIdAsync(orderId);
        if (order == null)
            throw new Exception("Order not found.");

        if (order.Status != OrderStatus.Pending)
            throw new Exception($"Cannot approve order. Current status is '{order.Status}'. Only 'Pending' orders can be approved.");

        // Get order details
        var orderDetails = await _orderDetailRepository.GetAsync(d => d.OrderId == orderId);
        if (!orderDetails.Any())
            throw new Exception("Order has no items.");

        // ===== BEGIN TRANSACTION =====
        await _unitOfWork.BeginTransactionAsync();

        try
        {
            foreach (var detail in orderDetails)
            {
                // 1. Check & Deduct from Warehouse Inventory
                var warehouseStock = (await _warehouseInventoryRepository
                    .GetAsync(w => w.ProductId == detail.ProductId))
                    .FirstOrDefault();

                if (warehouseStock == null)
                    throw new Exception($"Product ID {detail.ProductId} does not exist in warehouse inventory.");

                if (warehouseStock.Quantity < detail.Quantity)
                {
                    var product = await _productRepository.GetByIdAsync(detail.ProductId);
                    throw new Exception(
                        $"Insufficient warehouse stock for product '{product?.ProductName ?? detail.ProductId.ToString()}'. " +
                        $"Available: {warehouseStock.Quantity}, Requested: {detail.Quantity}");
                }

                warehouseStock.Quantity -= detail.Quantity;
                warehouseStock.LastUpdated = DateTime.UtcNow;
                _warehouseInventoryRepository.Update(warehouseStock);
            }

            // 2. Update order status to Processing
            order.Status = OrderStatus.Processing;
            order.UpdatedAt = DateTime.UtcNow;
            _orderRepository.Update(order);

            // 3. Commit transaction
            await _unitOfWork.CommitTransactionAsync();
        }
        catch
        {
            await _unitOfWork.RollbackTransactionAsync();
            throw;
        }
        // ===== END TRANSACTION =====

        return await BuildOrderDto(order);
    }

    public async Task<BranchOrderDto> ConfirmReceivedAsync(int orderId)
    {
        var order = await _orderRepository.GetByIdAsync(orderId);
        if (order == null)
            throw new Exception("Order not found.");

        if (order.Status != OrderStatus.Processing)
            throw new Exception($"Cannot confirm receipt. Current status is '{order.Status}'. Only 'Processing' orders can be confirmed.");

        // Get order details
        var orderDetails = await _orderDetailRepository.GetAsync(d => d.OrderId == orderId);
        if (!orderDetails.Any())
            throw new Exception("Order has no items.");

        // ===== BEGIN TRANSACTION =====
        await _unitOfWork.BeginTransactionAsync();

        try
        {
            foreach (var detail in orderDetails)
            {
                // Add to Branch Inventory (create if not exists)
                var branchStock = (await _branchInventoryRepository
                    .GetAsync(b => b.BranchId == order.BranchId && b.ProductId == detail.ProductId))
                    .FirstOrDefault();

                if (branchStock != null)
                {
                    branchStock.Quantity += detail.Quantity;
                    branchStock.LastUpdated = DateTime.UtcNow;
                    _branchInventoryRepository.Update(branchStock);
                }
                else
                {
                    var newBranchStock = new BranchInventory
                    {
                        BranchId = order.BranchId,
                        ProductId = detail.ProductId,
                        Quantity = detail.Quantity,
                        LastUpdated = DateTime.UtcNow
                    };
                    await _branchInventoryRepository.AddAsync(newBranchStock);
                }
            }

            // Update order status to Fulfilled
            order.Status = OrderStatus.Fulfilled;
            order.UpdatedAt = DateTime.UtcNow;
            _orderRepository.Update(order);

            // Commit transaction
            await _unitOfWork.CommitTransactionAsync();
        }
        catch
        {
            await _unitOfWork.RollbackTransactionAsync();
            throw;
        }
        // ===== END TRANSACTION =====

        return await BuildOrderDto(order);
    }

    public async Task<BranchOrderDto> CancelOrderAsync(int orderId)
    {
        var order = await _orderRepository.GetByIdAsync(orderId);
        if (order == null)
            throw new Exception("Order not found.");

        if (order.Status != OrderStatus.Pending)
            throw new Exception($"Cannot cancel order. Current status is '{order.Status}'. Only 'Pending' orders can be cancelled.");

        order.Status = OrderStatus.Cancelled;
        order.UpdatedAt = DateTime.UtcNow;
        _orderRepository.Update(order);
        await _unitOfWork.SaveChangesAsync();

        return await BuildOrderDto(order);
    }

    // ===== PRIVATE HELPER =====

    private async Task<BranchOrderDto> BuildOrderDto(BranchOrder order)
    {
        var branch = await _branchRepository.GetByIdAsync(order.BranchId);
        var creator = await _userRepository.GetByIdAsync(order.CreatedBy);
        var details = await _orderDetailRepository.GetAsync(d => d.OrderId == order.OrderId);

        var itemDtos = new List<BranchOrderDetailDto>();
        foreach (var detail in details)
        {
            var product = await _productRepository.GetByIdAsync(detail.ProductId);
            itemDtos.Add(new BranchOrderDetailDto
            {
                ProductId = detail.ProductId,
                ProductName = product?.ProductName ?? "",
                Quantity = detail.Quantity
            });
        }

        return new BranchOrderDto
        {
            OrderId = order.OrderId,
            BranchId = order.BranchId,
            BranchName = branch?.BranchName ?? "",
            CreatedBy = order.CreatedBy,
            CreatorName = creator?.FullName ?? "",
            Status = order.Status.ToString(),
            OrderDate = order.OrderDate,
            UpdatedAt = order.UpdatedAt,
            Items = itemDtos
        };
    }
}
