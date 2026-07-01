namespace ConvenienceStore.Domain.Entities;

public class ShiftAssignment
{
    public int AssignmentId { get; set; }
    public int ShiftId { get; set; }
    public int EmployeeId { get; set; }
    public DateTime WorkDate { get; set; }

    // Navigation properties
    public virtual Shift Shift { get; set; } = null!;
    public virtual User Employee { get; set; } = null!;
}
