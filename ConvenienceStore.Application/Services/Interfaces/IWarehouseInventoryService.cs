using ConvenienceStore.Application.DTOs.Inventory;

namespace ConvenienceStore.Application.Services.Interfaces;

public interface IWarehouseInventoryService
{
    Task<WarehouseInventoryListDto> GetAllWarehouseInventoryAsync(string? search, int page, int pageSize);
    Task<WarehouseInventoryDto?> GetWarehouseInventoryByProductAsync(int productId);
    Task<IEnumerable<WarehouseInventoryDto>> GetLowStockProductsAsync(int threshold);
    Task<StockAdjustmentResultDto> AdjustStockAsync(StockAdjustmentRequest request, System.Security.Claims.ClaimsPrincipal currentUser);
}
