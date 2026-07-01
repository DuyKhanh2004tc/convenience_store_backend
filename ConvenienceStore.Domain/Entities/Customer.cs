namespace ConvenienceStore.Domain.Entities;

public class Customer : BaseEntity
{
    public int CustomerId { get; set; }
    public string FullName { get; set; } = string.Empty;
    public string? Phone { get; set; }
    public string? Email { get; set; }
    public string? MembershipId { get; set; }
    public int LoyaltyPoints { get; set; } = 0;

    // Navigation properties
    public virtual ICollection<Invoice> Invoices { get; set; } = new List<Invoice>();
}
