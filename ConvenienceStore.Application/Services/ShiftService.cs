using ConvenienceStore.Application.Contracts.Persistence;
using ConvenienceStore.Application.DTOs.Shift;
using ConvenienceStore.Application.Services.Interfaces;
using ConvenienceStore.Domain.Entities;
using ConvenienceStore.Domain.Enums;

namespace ConvenienceStore.Application.Services;

public class ShiftService : IShiftService
{
    private readonly IGenericRepository<Shift> _shiftRepository;
    private readonly IGenericRepository<ShiftAssignment> _assignmentRepository;
    private readonly IGenericRepository<Branch> _branchRepository;
    private readonly IGenericRepository<User> _userRepository;
    private readonly IUnitOfWork _unitOfWork;

    public ShiftService(
        IGenericRepository<Shift> shiftRepository,
        IGenericRepository<ShiftAssignment> assignmentRepository,
        IGenericRepository<Branch> branchRepository,
        IGenericRepository<User> userRepository,
        IUnitOfWork unitOfWork)
    {
        _shiftRepository = shiftRepository;
        _assignmentRepository = assignmentRepository;
        _branchRepository = branchRepository;
        _userRepository = userRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<ShiftDto> CreateShiftAsync(CreateShiftDto dto)
    {
        var branch = await _branchRepository.GetByIdAsync(dto.BranchId);
        if (branch == null)
            throw new InvalidOperationException($"Branch with ID {dto.BranchId} not found.");

        var shift = new Shift
        {
            BranchId = dto.BranchId,
            ShiftName = dto.ShiftName,
            StartTime = dto.StartTime,
            EndTime = dto.EndTime
        };

        var created = await _shiftRepository.AddAsync(shift);
        await _unitOfWork.SaveChangesAsync();

        return new ShiftDto
        {
            ShiftId = created.ShiftId,
            BranchId = created.BranchId,
            BranchName = branch.BranchName,
            ShiftName = created.ShiftName,
            StartTime = created.StartTime,
            EndTime = created.EndTime
        };
    }

    public async Task<IEnumerable<ShiftDto>> GetShiftsByBranchAsync(int branchId)
    {
        var shifts = await _shiftRepository.GetAsync(s => s.BranchId == branchId);
        var branch = await _branchRepository.GetByIdAsync(branchId);

        return shifts.Select(s => new ShiftDto
        {
            ShiftId = s.ShiftId,
            BranchId = s.BranchId,
            BranchName = branch?.BranchName,
            ShiftName = s.ShiftName,
            StartTime = s.StartTime,
            EndTime = s.EndTime
        }).ToList();
    }

    public async Task<ShiftDto?> UpdateShiftAsync(int shiftId, UpdateShiftDto dto)
    {
        var shift = await _shiftRepository.GetByIdAsync(shiftId);
        if (shift == null) return null;

        shift.ShiftName = dto.ShiftName;
        shift.StartTime = dto.StartTime;
        shift.EndTime = dto.EndTime;
        _shiftRepository.Update(shift);
        await _unitOfWork.SaveChangesAsync();

        var branch = await _branchRepository.GetByIdAsync(shift.BranchId);
        return new ShiftDto
        {
            ShiftId = shift.ShiftId,
            BranchId = shift.BranchId,
            BranchName = branch?.BranchName,
            ShiftName = shift.ShiftName,
            StartTime = shift.StartTime,
            EndTime = shift.EndTime
        };
    }

    public async Task<bool> DeleteShiftAsync(int shiftId)
    {
        var shift = await _shiftRepository.GetByIdAsync(shiftId);
        if (shift == null) return false;

        var assignments = await _assignmentRepository.GetAsync(
            assignment => assignment.ShiftId == shiftId);
        if (assignments.Any())
            throw new InvalidOperationException(
                "Không thể xóa ca đã được sử dụng trong lịch làm việc.");

        _shiftRepository.Delete(shift);
        await _unitOfWork.SaveChangesAsync();
        return true;
    }

    public async Task<IEnumerable<ShiftAssignmentDto>> AssignShiftsAsync(AssignShiftDto dto)
    {
        if (dto.StartDate.Date < DateTime.Today || dto.EndDate.Date < DateTime.Today)
            throw new InvalidOperationException(
                "Không thể phân ca cho ngày trong quá khứ.");

        var shift = await _shiftRepository.GetByIdAsync(dto.ShiftId);
        if (shift == null)
            throw new InvalidOperationException($"Shift with ID {dto.ShiftId} not found.");

        if (dto.StartDate > dto.EndDate)
            throw new InvalidOperationException("StartDate must be before or equal to EndDate.");

        var createdAssignments = new List<ShiftAssignment>();

        // Iterate through each date in the range
        for (var date = dto.StartDate.Date; date <= dto.EndDate.Date; date = date.AddDays(1))
        {
            foreach (var employeeId in dto.EmployeeIds)
            {
                // Validate employee exists
                var employee = await _userRepository.GetByIdAsync(employeeId);
                if (employee == null)
                    throw new InvalidOperationException($"Employee with ID {employeeId} not found.");

                // Conflict detection: check if this employee already has a shift on this date
                // that overlaps with the target shift's time range
                var existingAssignments = await _assignmentRepository.GetAsync(
                    a => a.EmployeeId == employeeId
                         && a.WorkDate == date
                         && a.Status != ShiftStatus.Cancelled);

                // Check for time overlap with existing shifts
                foreach (var existing in existingAssignments)
                {
                    var existingShift = await _shiftRepository.GetByIdAsync(existing.ShiftId);
                    if (existingShift != null && HasTimeOverlap(shift.StartTime, shift.EndTime, existingShift.StartTime, existingShift.EndTime))
                    {
                        throw new InvalidOperationException(
                            $"Employee '{employee.FullName}' already has an overlapping shift " +
                            $"'{existingShift.ShiftName}' ({existingShift.StartTime:hh\\:mm}-{existingShift.EndTime:hh\\:mm}) " +
                            $"on {date:yyyy-MM-dd}.");
                    }
                }

                var assignment = new ShiftAssignment
                {
                    ShiftId = dto.ShiftId,
                    EmployeeId = employeeId,
                    WorkDate = date,
                    Status = ShiftStatus.Published
                };

                await _assignmentRepository.AddAsync(assignment);
                createdAssignments.Add(assignment);
            }
        }

        await _unitOfWork.SaveChangesAsync();

        // Build response
        var users = await _userRepository.GetAllAsync();
        return createdAssignments.Select(a => new ShiftAssignmentDto
        {
            AssignmentId = a.AssignmentId,
            EmployeeId = a.EmployeeId,
            EmployeeName = users.FirstOrDefault(u => u.UserId == a.EmployeeId)?.FullName,
            ShiftId = a.ShiftId,
            ShiftName = shift.ShiftName,
            WorkDate = a.WorkDate,
            Status = a.Status.ToString(),
            StartTime = shift.StartTime,
            EndTime = shift.EndTime
        }).ToList();
    }

    public async Task<ShiftAssignmentDto?> UpdateAssignmentAsync(
        int assignmentId, int employeeId)
    {
        var assignment = await _assignmentRepository.GetByIdAsync(assignmentId);
        var employee = await _userRepository.GetByIdAsync(employeeId);
        if (assignment == null || employee == null) return null;

        assignment.EmployeeId = employeeId;
        _assignmentRepository.Update(assignment);
        await _unitOfWork.SaveChangesAsync();

        var shift = await _shiftRepository.GetByIdAsync(assignment.ShiftId);
        return new ShiftAssignmentDto
        {
            AssignmentId = assignment.AssignmentId,
            EmployeeId = employee.UserId,
            EmployeeName = employee.FullName,
            ShiftId = assignment.ShiftId,
            ShiftName = shift?.ShiftName,
            WorkDate = assignment.WorkDate,
            Status = assignment.Status.ToString(),
            StartTime = shift?.StartTime ?? TimeSpan.Zero,
            EndTime = shift?.EndTime ?? TimeSpan.Zero
        };
    }

    public async Task<bool> DeleteAssignmentAsync(int assignmentId)
    {
        var assignment = await _assignmentRepository.GetByIdAsync(assignmentId);
        if (assignment == null) return false;
        assignment.Status = ShiftStatus.Cancelled;
        _assignmentRepository.Update(assignment);
        await _unitOfWork.SaveChangesAsync();
        return true;
    }

    public async Task<bool> AssignmentBelongsToBranchAsync(int assignmentId, int branchId)
    {
        var assignment = await _assignmentRepository.GetByIdAsync(assignmentId);
        if (assignment == null) return false;

        var shift = await _shiftRepository.GetByIdAsync(assignment.ShiftId);
        return shift?.BranchId == branchId;
    }

    public async Task<IEnumerable<ShiftAssignmentDto>> GetMyScheduleAsync(int employeeId, DateTime? startDate, DateTime? endDate)
    {
        var start = startDate ?? DateTime.UtcNow.Date;
        var end = endDate ?? start.AddDays(30);

        var assignments = await _assignmentRepository.GetAsync(
            a => a.EmployeeId == employeeId
                 && a.WorkDate >= start
                 && a.WorkDate <= end
                 && a.Status != ShiftStatus.Cancelled);

        var shifts = await _shiftRepository.GetAllAsync();
        var user = await _userRepository.GetByIdAsync(employeeId);

        return assignments.Select(a =>
        {
            var shift = shifts.FirstOrDefault(s => s.ShiftId == a.ShiftId);
            return new ShiftAssignmentDto
            {
                AssignmentId = a.AssignmentId,
                EmployeeId = a.EmployeeId,
                EmployeeName = user?.FullName,
                ShiftId = a.ShiftId,
                ShiftName = shift?.ShiftName,
                WorkDate = a.WorkDate,
                Status = a.Status.ToString(),
                StartTime = shift?.StartTime ?? TimeSpan.Zero,
                EndTime = shift?.EndTime ?? TimeSpan.Zero
            };
        }).OrderBy(a => a.WorkDate).ThenBy(a => a.StartTime).ToList();
    }

    public async Task<IEnumerable<ShiftAssignmentDto>> GetStoreScheduleAsync(int branchId, DateTime? startDate, DateTime? endDate)
    {
        var start = startDate ?? DateTime.UtcNow.Date;
        var end = endDate ?? start.AddDays(30);

        // Get all shifts for this branch
        var branchShifts = await _shiftRepository.GetAsync(s => s.BranchId == branchId);
        var shiftIds = branchShifts.Select(s => s.ShiftId).ToHashSet();

        // Get all assignments for those shifts within date range
        var allAssignments = await _assignmentRepository.GetAsync(
            a => shiftIds.Contains(a.ShiftId)
                 && a.WorkDate >= start
                 && a.WorkDate <= end
                 && a.Status != ShiftStatus.Cancelled);

        var users = await _userRepository.GetAllAsync();

        return allAssignments.Select(a =>
        {
            var shift = branchShifts.FirstOrDefault(s => s.ShiftId == a.ShiftId);
            return new ShiftAssignmentDto
            {
                AssignmentId = a.AssignmentId,
                EmployeeId = a.EmployeeId,
                EmployeeName = users.FirstOrDefault(u => u.UserId == a.EmployeeId)?.FullName,
                ShiftId = a.ShiftId,
                ShiftName = shift?.ShiftName,
                WorkDate = a.WorkDate,
                Status = a.Status.ToString(),
                StartTime = shift?.StartTime ?? TimeSpan.Zero,
                EndTime = shift?.EndTime ?? TimeSpan.Zero
            };
        }).OrderBy(a => a.WorkDate).ThenBy(a => a.StartTime).ThenBy(a => a.EmployeeName).ToList();
    }

    /// <summary>
    /// Checks if two time ranges overlap.
    /// </summary>
    private static bool HasTimeOverlap(TimeSpan start1, TimeSpan end1, TimeSpan start2, TimeSpan end2)
    {
        return start1 < end2 && start2 < end1;
    }
}
