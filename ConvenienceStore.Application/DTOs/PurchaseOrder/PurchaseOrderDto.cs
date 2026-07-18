namespace ConvenienceStore.Application.DTOs.PurchaseOrder;

public class PurchaseOrderDto
{
    public int PoId { get; set; }
    public int SupplierId { get; set; }
    public string SupplierName { get; set; } = string.Empty;
    public int CreatedBy { get; set; }
    public string CreatorName { get; set; } = string.Empty;
    public DateTime OrderDate { get; set; }
    public decimal TotalAmount { get; set; }
    public string Status { get; set; } = string.Empty;
    public List<PurchaseOrderItemDto> Items { get; set; } = new();
}
