namespace ConvenienceStore.Application.DTOs.GoodsReceipt;

public class CreateGoodsReceiptRequest
{
    public int PoId { get; set; }
    public string? Notes { get; set; }
    public List<GoodsReceiptItemDto> Items { get; set; } = new();
}
