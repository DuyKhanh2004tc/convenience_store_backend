using ConvenienceStore.Domain.Enums;

namespace ConvenienceStore.Domain.Entities;

public class Attendance
{
    public int AttendanceId { get; set; }
    public int ShiftAssignmentId { get; set; }

    // Check-in / Check-out timestamps
    public DateTime? CheckInTime { get; set; }
    public DateTime? CheckOutTime { get; set; }

    // Status per action
    public AttendanceStatus? CheckInStatus { get; set; }
    public AttendanceStatus? CheckOutStatus { get; set; }

    // Check-in location verification
    public string? CheckInIp { get; set; }
    public string? CheckInWifiBssid { get; set; }
    public double? CheckInLatitude { get; set; }
    public double? CheckInLongitude { get; set; }

    // Check-out location verification
    public string? CheckOutIp { get; set; }
    public double? CheckOutLatitude { get; set; }
    public double? CheckOutLongitude { get; set; }

    // Timing calculations
    public int LateMinutes { get; set; }
    public int EarlyMinutes { get; set; }

    // Navigation properties
    public virtual ShiftAssignment ShiftAssignment { get; set; } = null!;
}
