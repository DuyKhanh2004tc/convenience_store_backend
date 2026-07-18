namespace ConvenienceStore.Application.DTOs.Attendance;

public class CheckInRequestDto
{
    public int ShiftAssignmentId { get; set; }
    public double Latitude { get; set; }
    public double Longitude { get; set; }
    public string? WifiBssid { get; set; }
}

public class CheckOutRequestDto
{
    public int AttendanceId { get; set; }
    public double Latitude { get; set; }
    public double Longitude { get; set; }
}

public class ManualAttendanceRequestDto
{
    public int ShiftAssignmentId { get; set; }
    public DateTime CheckInTime { get; set; }
    public DateTime? CheckOutTime { get; set; }
}

public class AttendanceDto
{
    public int AttendanceId { get; set; }
    public int ShiftAssignmentId { get; set; }
    public int EmployeeId { get; set; }
    public string? EmployeeName { get; set; }
    public string? ShiftName { get; set; }
    public DateTime WorkDate { get; set; }
    public DateTime? CheckInTime { get; set; }
    public DateTime? CheckOutTime { get; set; }
    public string? CheckInStatus { get; set; }
    public string? CheckOutStatus { get; set; }
    public int LateMinutes { get; set; }
    public int EarlyMinutes { get; set; }
}

public class AttendanceReportDto
{
    public int EmployeeId { get; set; }
    public string EmployeeName { get; set; } = string.Empty;
    public int TotalShifts { get; set; }
    public int TotalPresent { get; set; }
    public int TotalLate { get; set; }
    public int TotalEarlyLeave { get; set; }
    public int TotalAbsent { get; set; }
    public int TotalLateMinutes { get; set; }
    public int TotalEarlyMinutes { get; set; }
}
