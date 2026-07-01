namespace ConvenienceStore.Domain.Entities;

public class BranchInventory
{
    public int BranchId { get; set; }
    public int ProductId { get; set; }
    public int Quantity { get; set; }
    public DateTime LastUpdated { get; set; } = DateTime.UtcNow;

    // Navigation properties
    public virtual Branch Branch { get; set; } = null!;
    public virtual Product Product { get; set; } = null!;
}
