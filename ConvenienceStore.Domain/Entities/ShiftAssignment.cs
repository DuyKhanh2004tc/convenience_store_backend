using ConvenienceStore.Domain.Enums;

namespace ConvenienceStore.Domain.Entities;

public class ShiftAssignment
{
    public int AssignmentId { get; set; }
    public int ShiftId { get; set; }
    public int EmployeeId { get; set; }
    public DateTime WorkDate { get; set; }
    public ShiftStatus Status { get; set; } = ShiftStatus.Draft;

    // Navigation properties
    public virtual Shift Shift { get; set; } = null!;
    public virtual User Employee { get; set; } = null!;
    public virtual Attendance? Attendance { get; set; }
}
