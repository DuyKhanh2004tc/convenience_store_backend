namespace ConvenienceStore.Application.DTOs.Inventory;

public class StockAdjustmentResultDto
{
    public int BranchId { get; set; }
    public string BranchName { get; set; } = string.Empty;
    public List<StockAdjustmentResultItemDto> AdjustedItems { get; set; } = new();
    public int TotalDelta => AdjustedItems.Sum(i => i.Delta);
}
