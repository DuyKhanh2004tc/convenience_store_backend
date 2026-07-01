using ConvenienceStore.Domain.Enums;

namespace ConvenienceStore.Domain.Entities;

public class Invoice
{
    public int InvoiceId { get; set; }
    public int BranchId { get; set; }
    public int StaffId { get; set; }
    public int? CustomerId { get; set; }
    public DateTime InvoiceDate { get; set; } = DateTime.UtcNow;
    public decimal TotalAmount { get; set; }
    public decimal DiscountAmount { get; set; }
    public decimal FinalAmount { get; set; }
    public string PaymentMethod { get; set; } = string.Empty;
    public OrderStatus Status { get; set; } = OrderStatus.Completed;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    // Navigation properties
    public virtual Branch Branch { get; set; } = null!;
    public virtual User Staff { get; set; } = null!;
    public virtual Customer? Customer { get; set; }
    public virtual ICollection<InvoiceDetail> InvoiceDetails { get; set; } = new List<InvoiceDetail>();
}
