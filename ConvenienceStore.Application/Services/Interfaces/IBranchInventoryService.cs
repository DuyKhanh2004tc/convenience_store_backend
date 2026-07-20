using ConvenienceStore.Application.DTOs.Inventory;
using System.Security.Claims;

namespace ConvenienceStore.Application.Services.Interfaces;

public interface IBranchInventoryService
{
    Task<BranchInventoryListDto> GetBranchInventoryAsync(int branchId, string? search, int page, int pageSize, ClaimsPrincipal currentUser);
    Task<BranchInventoryDto?> GetBranchInventoryByProductAsync(int branchId, int productId, ClaimsPrincipal currentUser);
    Task<IEnumerable<BranchInventoryDto>> GetLowStockByBranchAsync(int branchId, int threshold, ClaimsPrincipal currentUser);
    Task<IEnumerable<BranchInventoryDto>> GetAllLowStockAsync(int threshold);
    Task<StockAdjustmentResultDto> AdjustStockAsync(int branchId, StockAdjustmentRequest request, ClaimsPrincipal currentUser);
}
