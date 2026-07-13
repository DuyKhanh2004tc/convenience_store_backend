namespace ConvenienceStore.Domain.Entities;

public class Product : BaseEntity
{
    public int ProductId { get; set; }
    public string ProductName { get; set; } = string.Empty;
    public int CategoryId { get; set; }
    public int? SupplierId { get; set; }
    public string? Barcode { get; set; }
    public decimal CostPrice { get; set; }
    public decimal SellingPrice { get; set; }
    public bool IsActive { get; set; } = true;
    public string? ImageUrl { get; set; }
    public string? Unit { get; set; }
    public string? ImportUnit { get; set; }
    public int ConversionFactor { get; set; } = 1;

    // Navigation properties
    public virtual Category Category { get; set; } = null!;
    public virtual Supplier? Supplier { get; set; }
}
