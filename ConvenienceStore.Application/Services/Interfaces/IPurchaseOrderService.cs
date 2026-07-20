using ConvenienceStore.Application.DTOs.PurchaseOrder;
using ConvenienceStore.Application.DTOs.Common;

namespace ConvenienceStore.Application.Services.Interfaces;

public interface IPurchaseOrderService
{
    Task<PurchaseOrderDto> CreatePurchaseOrderAsync(CreatePurchaseOrderRequest request, int userId);
    Task<PagedResult<PurchaseOrderDto>> GetPurchaseOrdersAsync(
        int page,
        int pageSize,
        string? search,
        string? status);
    Task<PurchaseOrderDto?> GetPurchaseOrderByIdAsync(int id);
    Task<PurchaseOrderDto> CancelPurchaseOrderAsync(int id);
}
