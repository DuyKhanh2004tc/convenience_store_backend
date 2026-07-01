using ConvenienceStore.Domain.Enums;

namespace ConvenienceStore.Domain.Entities;

public class Attendance
{
    public int AttendanceId { get; set; }
    public int EmployeeId { get; set; }
    public DateTime WorkDate { get; set; }
    public DateTime? CheckInTime { get; set; }
    public DateTime? CheckOutTime { get; set; }
    public AttendanceStatus? Status { get; set; }

    // Navigation properties
    public virtual User Employee { get; set; } = null!;
}
