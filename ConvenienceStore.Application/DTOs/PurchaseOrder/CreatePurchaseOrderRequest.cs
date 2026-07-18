namespace ConvenienceStore.Application.DTOs.PurchaseOrder;

public class CreatePurchaseOrderRequest
{
    public int SupplierId { get; set; }
    public List<CreatePurchaseOrderItemDto> Items { get; set; } = new();
}
