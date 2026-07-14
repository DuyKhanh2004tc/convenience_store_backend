namespace ConvenienceStore.Application.DTOs.Invoice;

/// <summary>
/// Response DTO representing a completed invoice (receipt).
/// </summary>
public class InvoiceDto
{
    public int InvoiceId { get; set; }
    public int BranchId { get; set; }
    public string BranchName { get; set; } = string.Empty;
    public int StaffId { get; set; }
    public string StaffName { get; set; } = string.Empty;
    public int? CustomerId { get; set; }
    public string? CustomerName { get; set; }
    public string? CustomerPhone { get; set; }
    public DateTime InvoiceDate { get; set; }
    public decimal TotalAmount { get; set; }
    public decimal DiscountAmount { get; set; }
    public decimal FinalAmount { get; set; }
    public string PaymentMethod { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public int? LoyaltyPointsEarned { get; set; }
    public int? LoyaltyPointsRedeemed { get; set; }
    public int? CustomerLoyaltyBalance { get; set; }
    public DateTime CreatedAt { get; set; }
    public List<InvoiceDetailDto> Items { get; set; } = new();
}

public class InvoiceDetailDto
{
    public int ProductId { get; set; }
    public string ProductName { get; set; } = string.Empty;
    public string? Barcode { get; set; }
    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }
    public decimal SubTotal { get; set; }
}
