namespace ConvenienceStore.Domain.Entities;

public class Shift
{
    public int ShiftId { get; set; }
    public int BranchId { get; set; }
    public string ShiftName { get; set; } = string.Empty;
    public TimeSpan StartTime { get; set; }
    public TimeSpan EndTime { get; set; }

    // Navigation properties
    public virtual Branch Branch { get; set; } = null!;
    public virtual ICollection<ShiftAssignment> ShiftAssignments { get; set; } = new List<ShiftAssignment>();
}
