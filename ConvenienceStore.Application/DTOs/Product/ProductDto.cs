namespace ConvenienceStore.Application.DTOs.Product;

public class ProductDto
{
    public int ProductId { get; set; }
    public string ProductName { get; set; } = string.Empty;
    public int CategoryId { get; set; }
    public string CategoryName { get; set; } = string.Empty;
    public int? SupplierId { get; set; }
    public string? SupplierName { get; set; }
    public string? Barcode { get; set; }
    public decimal CostPrice { get; set; }
    public decimal SellingPrice { get; set; }
    public bool IsActive { get; set; }
    public string? ImageUrl { get; set; }
    public string? Unit { get; set; }
    public string? ImportUnit { get; set; }
    public int ConversionFactor { get; set; }
}

public class CreateProductDto
{
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
}

public class UpdateProductDto
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
}
