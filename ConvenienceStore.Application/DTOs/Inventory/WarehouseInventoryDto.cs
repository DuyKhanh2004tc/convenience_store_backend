namespace ConvenienceStore.Application.DTOs.Inventory;

public class WarehouseInventoryDto
{
    public int ProductId { get; set; }
    public string ProductName { get; set; } = string.Empty;
    public string? Barcode { get; set; }
    public string? Unit { get; set; }
    public string? ImportUnit { get; set; }
    public int ConversionFactor { get; set; }
    public int Quantity { get; set; }
    public DateTime LastUpdated { get; set; }
}
