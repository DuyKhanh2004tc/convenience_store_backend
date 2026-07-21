namespace ConvenienceStore.Application.DTOs.PurchaseOrder;

public class PurchaseOrderItemDto
{
    public int ProductId { get; set; }
    public string ProductName { get; set; } = string.Empty;
    public int Quantity { get; set; }
    public int QuantityReceived { get; set; }
    public int ShortageQuantity => Math.Max(0, Quantity - QuantityReceived);
    public decimal UnitPrice { get; set; }
    public decimal LineTotal => Quantity * UnitPrice;
}
