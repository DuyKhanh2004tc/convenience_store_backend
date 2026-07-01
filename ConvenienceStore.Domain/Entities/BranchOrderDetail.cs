namespace ConvenienceStore.Domain.Entities;

public class BranchOrderDetail
{
    public int OrderId { get; set; }
    public int ProductId { get; set; }
    public int Quantity { get; set; }

    // Navigation properties
    public virtual BranchOrder BranchOrder { get; set; } = null!;
    public virtual Product Product { get; set; } = null!;
}
