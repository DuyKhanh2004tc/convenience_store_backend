namespace ConvenienceStore.Application.DTOs.Inventory;

public class BranchInventoryDto
{
    public int BranchId { get; set; }
    public string BranchName { get; set; } = string.Empty;
    public int ProductId { get; set; }
    public string ProductName { get; set; } = string.Empty;
    public string? Barcode { get; set; }
    public string? Unit { get; set; }
    public int Quantity { get; set; }
    public DateTime LastUpdated { get; set; }
}
