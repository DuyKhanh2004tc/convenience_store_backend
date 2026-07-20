using ConvenienceStore.Application.Contracts.Persistence;
using ConvenienceStore.Application.DTOs.PurchaseOrder;
using ConvenienceStore.Application.DTOs.Common;
using ConvenienceStore.Application.Services.Interfaces;
using ConvenienceStore.Domain.Entities;
using ConvenienceStore.Domain.Enums;

namespace ConvenienceStore.Application.Services;

public class PurchaseOrderService : IPurchaseOrderService
{
    private readonly IGenericRepository<PurchaseOrder> _poRepository;
    private readonly IGenericRepository<PurchaseOrderDetail> _poDetailRepository;
    private readonly IGenericRepository<GoodsReceipt> _receiptRepository;
    private readonly IGenericRepository<GoodsReceiptDetail> _receiptDetailRepository;
    private readonly IGenericRepository<Product> _productRepository;
    private readonly IGenericRepository<Supplier> _supplierRepository;
    private readonly IGenericRepository<User> _userRepository;
    private readonly IUnitOfWork _unitOfWork;

    public PurchaseOrderService(
        IGenericRepository<PurchaseOrder> poRepository,
        IGenericRepository<PurchaseOrderDetail> poDetailRepository,
        IGenericRepository<GoodsReceipt> receiptRepository,
        IGenericRepository<GoodsReceiptDetail> receiptDetailRepository,
        IGenericRepository<Product> productRepository,
        IGenericRepository<Supplier> supplierRepository,
        IGenericRepository<User> userRepository,
        IUnitOfWork unitOfWork)
    {
        _poRepository = poRepository;
        _poDetailRepository = poDetailRepository;
        _receiptRepository = receiptRepository;
        _receiptDetailRepository = receiptDetailRepository;
        _productRepository = productRepository;
        _supplierRepository = supplierRepository;
        _userRepository = userRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<PurchaseOrderDto> CreatePurchaseOrderAsync(CreatePurchaseOrderRequest request, int userId)
    {
        var supplier = await _supplierRepository.GetByIdAsync(request.SupplierId);
        if (supplier == null) throw new Exception("Supplier not found.");
        if (!supplier.IsActive) throw new Exception("Supplier is inactive.");

        var creator = await _userRepository.GetByIdAsync(userId);
        if (creator == null || !creator.IsActive)
            throw new Exception("The purchase-order creator was not found or is inactive.");

        if (request.Items == null || !request.Items.Any())
            throw new Exception("Order items cannot be empty.");

        // Check duplicates
        var duplicates = request.Items.GroupBy(i => i.ProductId).Where(g => g.Count() > 1).Select(g => g.Key).ToList();
        if (duplicates.Any())
            throw new Exception($"Duplicate product IDs found: {string.Join(", ", duplicates)}");

        decimal totalAmount = 0;
        foreach (var item in request.Items)
        {
            if (item.Quantity <= 0) throw new Exception($"Quantity must be > 0 for product {item.ProductId}");
            if (item.UnitPrice < 0) throw new Exception($"UnitPrice must be >= 0 for product {item.ProductId}");

            var product = await _productRepository.GetByIdAsync(item.ProductId);
            if (product == null) throw new Exception($"Product ID {item.ProductId} not found.");
            if (!product.IsActive) throw new Exception($"Product {product.ProductName} is inactive.");

            totalAmount += item.Quantity * item.UnitPrice;
        }

        await _unitOfWork.BeginTransactionAsync();

        var order = new PurchaseOrder();
        try
        {
            order.SupplierId = request.SupplierId;
            order.CreatedBy = userId;
            order.OrderDate = DateTime.UtcNow;
            order.TotalAmount = totalAmount;
            order.Status = PurchaseOrderStatus.Pending;
            order.UpdatedAt = DateTime.UtcNow;

            await _poRepository.AddAsync(order);
            await _unitOfWork.SaveChangesAsync(); // Save to get PoId inside the transaction.

            var details = request.Items.Select(item => new PurchaseOrderDetail
            {
                PoId = order.PoId,
                ProductId = item.ProductId,
                Quantity = item.Quantity,
                UnitPrice = item.UnitPrice
            }).ToList();

            await _poDetailRepository.AddRangeAsync(details);
            await _unitOfWork.CommitTransactionAsync();
        }
        catch
        {
            await _unitOfWork.RollbackTransactionAsync();
            throw;
        }

        return await BuildPurchaseOrderDto(order);
    }

    public async Task<PagedResult<PurchaseOrderDto>> GetPurchaseOrdersAsync(
        int page,
        int pageSize,
        string? search,
        string? status)
    {
        var orders = await _poRepository.GetAllAsync();
        var suppliers = (await _supplierRepository.GetAllAsync())
            .ToDictionary(supplier => supplier.SupplierId);

        var filteredOrders = orders.AsEnumerable();

        if (!string.IsNullOrWhiteSpace(status))
        {
            if (!Enum.TryParse<PurchaseOrderStatus>(status.Trim(), true, out var parsedStatus))
                throw new ArgumentException("Trạng thái PO không hợp lệ. Chỉ chấp nhận Pending, Received hoặc Cancelled.");

            filteredOrders = filteredOrders.Where(order => order.Status == parsedStatus);
        }

        if (!string.IsNullOrWhiteSpace(search))
        {
            var normalizedSearch = search.Trim();
            filteredOrders = filteredOrders.Where(order =>
                order.PoId.ToString().Contains(normalizedSearch, StringComparison.OrdinalIgnoreCase) ||
                $"PO-{order.OrderDate:yyyyMMdd}-{order.PoId:D6}"
                    .Contains(normalizedSearch, StringComparison.OrdinalIgnoreCase) ||
                (suppliers.TryGetValue(order.SupplierId, out var supplier) &&
                 supplier.SupplierName.Contains(normalizedSearch, StringComparison.OrdinalIgnoreCase)));
        }

        var orderedResults = filteredOrders
            .OrderByDescending(order => order.OrderDate)
            .ToList();
        var totalCount = orderedResults.Count;
        var pageOrders = orderedResults
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToList();
        var result = new List<PurchaseOrderDto>();

        foreach (var order in pageOrders)
        {
            result.Add(await BuildPurchaseOrderDto(order));
        }

        return new PagedResult<PurchaseOrderDto>
        {
            Items = result,
            TotalCount = totalCount,
            PageNumber = page,
            PageSize = pageSize
        };
    }

    public async Task<PurchaseOrderDto?> GetPurchaseOrderByIdAsync(int id)
    {
        var order = await _poRepository.GetByIdAsync(id);
        if (order == null) return null;

        return await BuildPurchaseOrderDto(order);
    }

    public async Task<PurchaseOrderDto> CancelPurchaseOrderAsync(int id)
    {
        var order = await _poRepository.GetByIdAsync(id);
        if (order == null) throw new Exception("Purchase Order not found.");

        if (order.Status != PurchaseOrderStatus.Pending)
            throw new Exception($"Cannot cancel PO. Current status is {order.Status}");

        order.Status = PurchaseOrderStatus.Cancelled;
        order.UpdatedAt = DateTime.UtcNow;
        _poRepository.Update(order);
        
        await _unitOfWork.SaveChangesAsync();

        return await BuildPurchaseOrderDto(order);
    }

    private async Task<PurchaseOrderDto> BuildPurchaseOrderDto(PurchaseOrder order)
    {
        var supplier = await _supplierRepository.GetByIdAsync(order.SupplierId);
        var creator = await _userRepository.GetByIdAsync(order.CreatedBy);
        var details = await _poDetailRepository.GetAsync(d => d.PoId == order.PoId);
        var receipt = (await _receiptRepository.GetAsync(r => r.PoId == order.PoId)).FirstOrDefault();
        var receiptDetails = receipt == null
            ? new List<GoodsReceiptDetail>()
            : (await _receiptDetailRepository.GetAsync(d => d.ReceiptId == receipt.ReceiptId)).ToList();

        var itemDtos = new List<PurchaseOrderItemDto>();
        foreach (var detail in details)
        {
            var product = await _productRepository.GetByIdAsync(detail.ProductId);
            itemDtos.Add(new PurchaseOrderItemDto
            {
                ProductId = detail.ProductId,
                ProductName = product?.ProductName ?? "",
                Quantity = detail.Quantity,
                QuantityReceived = receiptDetails.FirstOrDefault(d => d.ProductId == detail.ProductId)?.QuantityReceived ?? 0,
                UnitPrice = detail.UnitPrice
            });
        }

        return new PurchaseOrderDto
        {
            PoId = order.PoId,
            SupplierId = order.SupplierId,
            SupplierName = supplier?.SupplierName ?? "",
            CreatedBy = order.CreatedBy,
            CreatorName = creator?.FullName ?? "",
            OrderDate = order.OrderDate,
            TotalAmount = order.TotalAmount,
            Status = order.Status.ToString(),
            ReceiptId = receipt?.ReceiptId,
            Items = itemDtos
        };
    }
}
