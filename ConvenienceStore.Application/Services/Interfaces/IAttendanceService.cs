using ConvenienceStore.Application.DTOs.Attendance;

namespace ConvenienceStore.Application.Services.Interfaces;

public interface IAttendanceService
{
    Task<AttendanceDto> CheckInAsync(int employeeId, CheckInRequestDto dto);
    Task<AttendanceDto> CheckOutAsync(int employeeId, CheckOutRequestDto dto);
    Task<AttendanceDto?> GetTodayAttendanceAsync(int employeeId);
    Task<IEnumerable<AttendanceReportDto>> GetAttendanceReportAsync(int branchId, DateTime startDate, DateTime endDate);
    Task<IEnumerable<AttendanceDto>> GetAttendanceHistoryAsync(int employeeId, DateTime startDate, DateTime endDate);
    Task<AttendanceDto> SaveManualAttendanceAsync(ManualAttendanceRequestDto dto);
}
