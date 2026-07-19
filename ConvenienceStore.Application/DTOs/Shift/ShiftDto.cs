namespace ConvenienceStore.Application.DTOs.Shift;

public class ShiftDto
{
    public int ShiftId { get; set; }
    public int BranchId { get; set; }
    public string? BranchName { get; set; }
    public string ShiftName { get; set; } = string.Empty;
    public TimeSpan StartTime { get; set; }
    public TimeSpan EndTime { get; set; }
}

public class CreateShiftDto
{
    public int BranchId { get; set; }
    public string ShiftName { get; set; } = string.Empty;
    public TimeSpan StartTime { get; set; }
    public TimeSpan EndTime { get; set; }
}

public class UpdateShiftDto
{
    public string ShiftName { get; set; } = string.Empty;
    public TimeSpan StartTime { get; set; }
    public TimeSpan EndTime { get; set; }
}

public class AssignShiftDto
{
    public int[] EmployeeIds { get; set; } = Array.Empty<int>();
    public int ShiftId { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
}

public class UpdateShiftAssignmentDto
{
    public int EmployeeId { get; set; }
}

public class ShiftAssignmentDto
{
    public int AssignmentId { get; set; }
    public int EmployeeId { get; set; }
    public string? EmployeeName { get; set; }
    public int ShiftId { get; set; }
    public string? ShiftName { get; set; }
    public DateTime WorkDate { get; set; }
    public string Status { get; set; } = string.Empty;
    public TimeSpan StartTime { get; set; }
    public TimeSpan EndTime { get; set; }
}
