namespace ConvenienceStore.Application.DTOs.GoodsReceipt;

public class GoodsReceiptDto
{
    public int ReceiptId { get; set; }
    public int PoId { get; set; }
    public int ReceivedBy { get; set; }
    public string ReceiverName { get; set; } = string.Empty;
    public DateTime ReceivedDate { get; set; }
    public string? Notes { get; set; }
    public List<GoodsReceiptItemDto> Items { get; set; } = new();
    public bool HasShortage => Items.Any(item => item.ShortageQuantity > 0);
}
