namespace ConvenienceStore.Application.DTOs.Inventory;

public class StockAdjustmentItemDto
{
    public int ProductId { get; set; }
    public int ExpectedQuantity { get; set; }
    public int ActualQuantity { get; set; }
    public string Reason { get; set; } = string.Empty;
    public string? Note { get; set; }
}
