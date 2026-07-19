using ConvenienceStore.Application.Contracts.Persistence;
using ConvenienceStore.Application.DTOs.Inventory;
using ConvenienceStore.Application.Services.Interfaces;
using ConvenienceStore.Domain.Entities;
using System.Security.Claims;

namespace ConvenienceStore.Application.Services;

public class WarehouseInventoryService : IWarehouseInventoryService
{
    private readonly IGenericRepository<WarehouseInventory> _warehouseInventoryRepository;
    private readonly IGenericRepository<Product> _productRepository;
    private readonly INotificationService _notificationService;
    private readonly IUnitOfWork _unitOfWork;

    public WarehouseInventoryService(
        IGenericRepository<WarehouseInventory> warehouseInventoryRepository,
        IGenericRepository<Product> productRepository,
        INotificationService notificationService,
        IUnitOfWork unitOfWork)
    {
        _warehouseInventoryRepository = warehouseInventoryRepository;
        _productRepository = productRepository;
        _notificationService = notificationService;
        _unitOfWork = unitOfWork;
    }

    private int GetCurrentUserId(System.Security.Claims.ClaimsPrincipal currentUser)
    {
        var idClaim = currentUser.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier);
        if (idClaim != null && int.TryParse(idClaim.Value, out int id)) return id;
        return 0; // System user
    }

    private string GetUserFullName(System.Security.Claims.ClaimsPrincipal currentUser)
    {
        var nameClaim = currentUser.FindFirst("FullName") ?? currentUser.FindFirst(System.Security.Claims.ClaimTypes.Name);
        return nameClaim?.Value ?? "System Admin";
    }

    public async Task<WarehouseInventoryListDto> GetAllWarehouseInventoryAsync(string? search, int page, int pageSize)
    {
        var query = await _warehouseInventoryRepository.GetAllAsync();
        
        // Manual join with product to get search fields
        var allProducts = await _productRepository.GetAllAsync();

        var joined = query.Join(allProducts, 
            wi => wi.ProductId, 
            p => p.ProductId, 
            (wi, p) => new { wi, p });

        if (!string.IsNullOrEmpty(search))
        {
            var lowerSearch = search.ToLower();
            joined = joined.Where(x => 
                x.p.ProductName.ToLower().Contains(lowerSearch) || 
                (x.p.Barcode != null && x.p.Barcode.ToLower().Contains(lowerSearch)));
        }

        var totalCount = joined.Count();
        
        var items = joined
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(x => new WarehouseInventoryDto
            {
                ProductId = x.wi.ProductId,
                ProductName = x.p.ProductName,
                Barcode = x.p.Barcode,
                Unit = x.p.Unit,
                ImportUnit = x.p.ImportUnit,
                ConversionFactor = x.p.ConversionFactor,
                Quantity = x.wi.Quantity,
                LastUpdated = x.wi.LastUpdated
            })
            .ToList();

        return new WarehouseInventoryListDto
        {
            Items = items,
            Page = page,
            PageSize = pageSize,
            TotalCount = totalCount
        };
    }

    public async Task<WarehouseInventoryDto?> GetWarehouseInventoryByProductAsync(int productId)
    {
        var inventory = (await _warehouseInventoryRepository.GetAsync(wi => wi.ProductId == productId)).FirstOrDefault();
        if (inventory == null) return null;

        var product = await _productRepository.GetByIdAsync(productId);
        if (product == null) return null;

        return new WarehouseInventoryDto
        {
            ProductId = inventory.ProductId,
            ProductName = product.ProductName,
            Barcode = product.Barcode,
            Unit = product.Unit,
            ImportUnit = product.ImportUnit,
            ConversionFactor = product.ConversionFactor,
            Quantity = inventory.Quantity,
            LastUpdated = inventory.LastUpdated
        };
    }

    public async Task<IEnumerable<WarehouseInventoryDto>> GetLowStockProductsAsync(int threshold)
    {
        var query = await _warehouseInventoryRepository.GetAsync(wi => wi.Quantity <= threshold);
        
        var allProducts = await _productRepository.GetAllAsync();

        var joined = query.Join(allProducts, 
            wi => wi.ProductId, 
            p => p.ProductId, 
            (wi, p) => new WarehouseInventoryDto
            {
                ProductId = wi.ProductId,
                ProductName = p.ProductName,
                Barcode = p.Barcode,
                Unit = p.Unit,
                ImportUnit = p.ImportUnit,
                ConversionFactor = p.ConversionFactor,
                Quantity = wi.Quantity,
                LastUpdated = wi.LastUpdated
            }).ToList();

        return joined;
    }

    public async Task<StockAdjustmentResultDto> AdjustStockAsync(StockAdjustmentRequest request, System.Security.Claims.ClaimsPrincipal currentUser)
    {
        var role = currentUser.FindFirstValue(System.Security.Claims.ClaimTypes.Role);
        if (role != "Warehouse Staff" && role != "Chain Director" && role != "System Admin")
        {
            throw new UnauthorizedAccessException("You do not have permission to adjust warehouse stock.");
        }

        if (request.Items == null || !request.Items.Any())
            throw new Exception("Adjustment request items cannot be empty.");

        var duplicates = request.Items.GroupBy(i => i.ProductId).Where(g => g.Count() > 1).Select(g => g.Key).ToList();
        if (duplicates.Any())
            throw new Exception($"Duplicate product IDs found: {string.Join(", ", duplicates)}");

        var userId = GetCurrentUserId(currentUser);
        var userName = GetUserFullName(currentUser);

        var result = new StockAdjustmentResultDto
        {
            BranchId = 0,
            BranchName = "Warehouse"
        };

        await _unitOfWork.BeginTransactionAsync();

        try
        {
            foreach (var item in request.Items)
            {
                if (item.ActualQuantity < 0)
                    throw new Exception($"ActualQuantity cannot be negative for product {item.ProductId}");

                if (!ConvenienceStore.Application.Constants.AdjustmentReasons.IsValid(item.Reason))
                    throw new Exception($"Invalid reason '{item.Reason}' for product {item.ProductId}");

                var inventory = (await _warehouseInventoryRepository.GetAsync(wi => wi.ProductId == item.ProductId)).FirstOrDefault();
                if (inventory == null)
                    throw new Exception($"Inventory record not found for product {item.ProductId} in warehouse");

                var oldQuantity = inventory.Quantity;
                var delta = item.ActualQuantity - oldQuantity;

                if (delta == 0) continue; // No change

                inventory.Quantity = item.ActualQuantity;
                inventory.LastUpdated = DateTime.UtcNow;

                _warehouseInventoryRepository.Update(inventory);

                var product = await _productRepository.GetByIdAsync(item.ProductId);
                var productName = product?.ProductName ?? $"Unknown ({item.ProductId})";

                var title = $"Stock Adjustment - {productName}";
                if (title.Length > 255) title = title[..255];

                var auditMessage = $@"Type: StockAdjustment
Warehouse: Total Warehouse
Product: {productName} (ID: {item.ProductId})
System Quantity: {oldQuantity}
Actual Quantity: {item.ActualQuantity}
Delta: {delta}
Reason: {item.Reason}
Adjusted By: {userName} (ID: {userId})
Adjusted At: {DateTime.UtcNow:O}";

                await _notificationService.CreateAuditLogAsync(userId, title, auditMessage);

                result.AdjustedItems.Add(new StockAdjustmentResultItemDto
                {
                    ProductId = item.ProductId,
                    ProductName = productName,
                    OldQuantity = oldQuantity,
                    ActualQuantity = item.ActualQuantity,
                    Delta = delta,
                    Reason = item.Reason,
                    AuditMessage = auditMessage
                });
            }

            await _unitOfWork.CommitTransactionAsync();
            return result;
        }
        catch
        {
            await _unitOfWork.RollbackTransactionAsync();
            throw;
        }
    }
}
