using ConvenienceStore.Application.DTOs.PurchaseOrder;

namespace ConvenienceStore.Application.Services.Interfaces;

public interface IPurchaseOrderService
{
    Task<PurchaseOrderDto> CreatePurchaseOrderAsync(CreatePurchaseOrderRequest request, int userId);
    Task<IEnumerable<PurchaseOrderDto>> GetAllPurchaseOrdersAsync();
    Task<PurchaseOrderDto?> GetPurchaseOrderByIdAsync(int id);
    Task<PurchaseOrderDto> CancelPurchaseOrderAsync(int id);
}
