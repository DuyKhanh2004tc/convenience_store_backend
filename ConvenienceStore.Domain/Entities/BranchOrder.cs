using ConvenienceStore.Domain.Enums;

namespace ConvenienceStore.Domain.Entities;

public class BranchOrder
{
    public int OrderId { get; set; }
    public int BranchId { get; set; }
    public int CreatedBy { get; set; }
    public DateTime OrderDate { get; set; } = DateTime.UtcNow;
    public OrderStatus Status { get; set; } = OrderStatus.Pending;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    // Navigation properties
    public virtual Branch Branch { get; set; } = null!;
    public virtual User Creator { get; set; } = null!;
    public virtual ICollection<BranchOrderDetail> BranchOrderDetails { get; set; } = new List<BranchOrderDetail>();
}
