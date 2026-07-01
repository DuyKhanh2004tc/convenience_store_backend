namespace ConvenienceStore.Domain.Entities;

public class GoodsReceiptDetail
{
    public int ReceiptId { get; set; }
    public int ProductId { get; set; }
    public int QuantityReceived { get; set; }

    // Navigation properties
    public virtual GoodsReceipt GoodsReceipt { get; set; } = null!;
    public virtual Product Product { get; set; } = null!;
}
