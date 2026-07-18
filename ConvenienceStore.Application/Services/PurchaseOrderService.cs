using ConvenienceStore.Application.Contracts.Persistence;
using ConvenienceStore.Application.DTOs.PurchaseOrder;
using ConvenienceStore.Application.Services.Interfaces;
using ConvenienceStore.Domain.Entities;
using ConvenienceStore.Domain.Enums;

namespace ConvenienceStore.Application.Services;

public class PurchaseOrderService : IPurchaseOrderService
{
    private readonly IGenericRepository<PurchaseOrder> _poRepository;
    private readonly IGenericRepository<PurchaseOrderDetail> _poDetailRepository;
    private readonly IGenericRepository<Product> _productRepository;
    private readonly IGenericRepository<Supplier> _supplierRepository;
    private readonly IGenericRepository<User> _userRepository;
    private readonly IUnitOfWork _unitOfWork;

    public PurchaseOrderService(
        IGenericRepository<PurchaseOrder> poRepository,
        IGenericRepository<PurchaseOrderDetail> poDetailRepository,
        IGenericRepository<Product> productRepository,
        IGenericRepository<Supplier> supplierRepository,
        IGenericRepository<User> userRepository,
        IUnitOfWork unitOfWork)
    {
        _poRepository = poRepository;
        _poDetailRepository = poDetailRepository;
        _productRepository = productRepository;
        _supplierRepository = supplierRepository;
        _userRepository = userRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<PurchaseOrderDto> CreatePurchaseOrderAsync(CreatePurchaseOrderRequest request, int userId)
    {
        var supplier = await _supplierRepository.GetByIdAsync(request.SupplierId);
        if (supplier == null) throw new Exception("Supplier not found.");

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

        var order = new PurchaseOrder
        {
            SupplierId = request.SupplierId,
            CreatedBy = userId,
            OrderDate = DateTime.UtcNow,
            TotalAmount = totalAmount,
            Status = PurchaseOrderStatus.Pending,
            UpdatedAt = DateTime.UtcNow
        };

        await _poRepository.AddAsync(order);
        await _unitOfWork.SaveChangesAsync(); // save to get PoId

        var details = request.Items.Select(item => new PurchaseOrderDetail
        {
            PoId = order.PoId,
            ProductId = item.ProductId,
            Quantity = item.Quantity,
            UnitPrice = item.UnitPrice
        }).ToList();

        await _poDetailRepository.AddRangeAsync(details);
        await _unitOfWork.SaveChangesAsync();

        return await BuildPurchaseOrderDto(order);
    }

    public async Task<IEnumerable<PurchaseOrderDto>> GetAllPurchaseOrdersAsync()
    {
        var orders = await _poRepository.GetAllAsync();
        var result = new List<PurchaseOrderDto>();
        
        foreach (var order in orders.OrderByDescending(o => o.OrderDate))
        {
            result.Add(await BuildPurchaseOrderDto(order));
        }

        return result;
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

        var itemDtos = new List<PurchaseOrderItemDto>();
        foreach (var detail in details)
        {
            var product = await _productRepository.GetByIdAsync(detail.ProductId);
            itemDtos.Add(new PurchaseOrderItemDto
            {
                ProductId = detail.ProductId,
                ProductName = product?.ProductName ?? "",
                Quantity = detail.Quantity,
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
            Items = itemDtos
        };
    }
}
