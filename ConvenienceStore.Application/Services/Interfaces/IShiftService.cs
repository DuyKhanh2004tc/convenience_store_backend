using ConvenienceStore.Application.DTOs.Shift;

namespace ConvenienceStore.Application.Services.Interfaces;

public interface IShiftService
{
    Task<ShiftDto> CreateShiftAsync(CreateShiftDto dto);
    Task<ShiftDto?> UpdateShiftAsync(int shiftId, UpdateShiftDto dto);
    Task<bool> DeleteShiftAsync(int shiftId);
    Task<IEnumerable<ShiftDto>> GetShiftsByBranchAsync(int branchId);
    Task<IEnumerable<ShiftAssignmentDto>> AssignShiftsAsync(AssignShiftDto dto);
    Task<ShiftAssignmentDto?> UpdateAssignmentAsync(int assignmentId, int employeeId);
    Task<bool> DeleteAssignmentAsync(int assignmentId);
    Task<IEnumerable<ShiftAssignmentDto>> GetMyScheduleAsync(int employeeId, DateTime? startDate, DateTime? endDate);
    Task<IEnumerable<ShiftAssignmentDto>> GetStoreScheduleAsync(int branchId, DateTime? startDate, DateTime? endDate);
}
