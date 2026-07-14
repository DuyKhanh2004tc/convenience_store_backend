using System.ComponentModel.DataAnnotations;

namespace ConvenienceStore.Application.DTOs.Invoice;

/// <summary>
/// Request DTO for POS checkout – create a new sales invoice.
/// </summary>
public class CreateInvoiceDto
{
    [Required]
    public int BranchId { get; set; }

    /// <summary>
    /// Optional – if provided, loyalty points will be calculated.
    /// </summary>
    public int? CustomerId { get; set; }

    [Required]
    [MinLength(1, ErrorMessage = "Invoice must contain at least one item.")]
    public List<CreateInvoiceItemDto> Items { get; set; } = new();

    /// <summary>
    /// "Cash", "Card", "QR", etc.
    /// </summary>
    [Required]
    public string PaymentMethod { get; set; } = "Cash";

    /// <summary>
    /// Number of loyalty points the customer wants to redeem (0 if not using).
    /// Each point = 1,000 VND discount.
    /// </summary>
    public int RedeemPoints { get; set; } = 0;
}

public class CreateInvoiceItemDto
{
    [Required]
    public int ProductId { get; set; }

    [Required]
    [Range(1, int.MaxValue, ErrorMessage = "Quantity must be at least 1.")]
    public int Quantity { get; set; }
}
