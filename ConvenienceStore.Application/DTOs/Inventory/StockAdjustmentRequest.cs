namespace ConvenienceStore.Application.DTOs.Inventory;

public class StockAdjustmentRequest
{
    public List<StockAdjustmentItemDto> Items { get; set; } = new();
}
