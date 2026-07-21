namespace ConvenienceStore.Application.DTOs.GoodsReceipt;

public class GoodsReceiptItemDto
{
    public int ProductId { get; set; }
    public string ProductName { get; set; } = string.Empty;
    public int OrderedQuantity { get; set; }
    public int QuantityReceived { get; set; }
    public int ShortageQuantity => Math.Max(0, OrderedQuantity - QuantityReceived);
    public string? Unit { get; set; }
    public string? ImportUnit { get; set; }
    public int ConversionFactor { get; set; }
}
