using ConvenienceStore.Application.Contracts.Persistence;
using ConvenienceStore.Application.DTOs.Inventory;
using ConvenienceStore.Application.Services.Interfaces;
using ConvenienceStore.Domain.Entities;

namespace ConvenienceStore.Application.Services;

public class WarehouseInventoryService : IWarehouseInventoryService
{
    private readonly IGenericRepository<WarehouseInventory> _warehouseInventoryRepository;
    private readonly IGenericRepository<Product> _productRepository;

    public WarehouseInventoryService(
        IGenericRepository<WarehouseInventory> warehouseInventoryRepository,
        IGenericRepository<Product> productRepository)
    {
        _warehouseInventoryRepository = warehouseInventoryRepository;
        _productRepository = productRepository;
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
}
