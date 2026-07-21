using ConvenienceStore.Application.DTOs.Inventory;

namespace ConvenienceStore.Application.Services.Interfaces;

public interface IWarehouseInventoryService
{
    Task<WarehouseInventoryListDto> GetAllWarehouseInventoryAsync(
        string? search,
        int page,
        int pageSize,
        bool lowStockOnly,
        int lowStockThreshold);
    Task<WarehouseInventoryDto?> GetWarehouseInventoryByProductAsync(int productId);
    Task<IEnumerable<WarehouseInventoryDto>> GetLowStockProductsAsync(int threshold);
}
