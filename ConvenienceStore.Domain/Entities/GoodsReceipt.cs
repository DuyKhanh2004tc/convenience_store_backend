namespace ConvenienceStore.Domain.Entities;

public class GoodsReceipt
{
    public int ReceiptId { get; set; }
    public int PoId { get; set; }
    public int ReceivedBy { get; set; }
    public DateTime ReceivedDate { get; set; } = DateTime.UtcNow;
    public string? Notes { get; set; }

    // Navigation properties
    public virtual PurchaseOrder PurchaseOrder { get; set; } = null!;
    public virtual User Receiver { get; set; } = null!;
    public virtual ICollection<GoodsReceiptDetail> GoodsReceiptDetails { get; set; } = new List<GoodsReceiptDetail>();
}
