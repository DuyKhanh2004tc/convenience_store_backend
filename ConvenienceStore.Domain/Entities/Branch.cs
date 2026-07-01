namespace ConvenienceStore.Domain.Entities;

public class Branch : BaseEntity
{
    public int BranchId { get; set; }
    public string BranchName { get; set; } = string.Empty;
    public string? Address { get; set; }
    public string? Phone { get; set; }
    public bool IsActive { get; set; } = true;

    // Navigation properties
    public virtual ICollection<User> Users { get; set; } = new List<User>();
    public virtual ICollection<BranchInventory> BranchInventories { get; set; } = new List<BranchInventory>();
    public virtual ICollection<BranchOrder> BranchOrders { get; set; } = new List<BranchOrder>();
    public virtual ICollection<Invoice> Invoices { get; set; } = new List<Invoice>();
    public virtual ICollection<Shift> Shifts { get; set; } = new List<Shift>();
}
