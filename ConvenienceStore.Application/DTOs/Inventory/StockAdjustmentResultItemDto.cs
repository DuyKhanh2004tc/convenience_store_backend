namespace ConvenienceStore.Application.DTOs.Inventory;

public class StockAdjustmentResultItemDto
{
    public int ProductId { get; set; }
    public string ProductName { get; set; } = string.Empty;
    public int OldQuantity { get; set; }
    public int ActualQuantity { get; set; }
    public int Delta { get; set; }
    public string Reason { get; set; } = string.Empty;
    public string? Note { get; set; }
    public string AuditMessage { get; set; } = string.Empty;
}
