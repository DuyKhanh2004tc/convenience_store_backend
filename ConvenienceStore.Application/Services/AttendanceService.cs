using ConvenienceStore.Application.Contracts.Persistence;
using ConvenienceStore.Application.DTOs.Attendance;
using ConvenienceStore.Application.Services.Interfaces;
using ConvenienceStore.Domain.Entities;
using ConvenienceStore.Domain.Enums;

namespace ConvenienceStore.Application.Services;

public class AttendanceService : IAttendanceService
{
    private readonly IGenericRepository<Attendance> _attendanceRepository;
    private readonly IGenericRepository<ShiftAssignment> _assignmentRepository;
    private readonly IGenericRepository<Shift> _shiftRepository;
    private readonly IGenericRepository<Branch> _branchRepository;
    private readonly IGenericRepository<User> _userRepository;
    private readonly IUnitOfWork _unitOfWork;

    // Maximum allowed distance from store in meters
    private const double MaxDistanceMeters = 50.0;
    private const int CheckInOpenMinutes = 20;
    private const int LateGraceMinutes = 15;

    public AttendanceService(
        IGenericRepository<Attendance> attendanceRepository,
        IGenericRepository<ShiftAssignment> assignmentRepository,
        IGenericRepository<Shift> shiftRepository,
        IGenericRepository<Branch> branchRepository,
        IGenericRepository<User> userRepository,
        IUnitOfWork unitOfWork)
    {
        _attendanceRepository = attendanceRepository;
        _assignmentRepository = assignmentRepository;
        _shiftRepository = shiftRepository;
        _branchRepository = branchRepository;
        _userRepository = userRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<AttendanceDto> CheckInAsync(int employeeId, CheckInRequestDto dto)
    {
        // 1. Validate the shift assignment exists and belongs to this employee
        var assignment = await _assignmentRepository.GetByIdAsync(dto.ShiftAssignmentId);
        if (assignment == null)
            throw new InvalidOperationException("Shift assignment not found.");

        if (assignment.EmployeeId != employeeId)
            throw new InvalidOperationException("This shift assignment does not belong to you.");

        if (assignment.Status == ShiftStatus.Cancelled)
            throw new InvalidOperationException("This shift assignment has been cancelled.");

        // 2. Check if already checked in
        var existingAttendance = await _attendanceRepository.GetAsync(
            a => a.ShiftAssignmentId == dto.ShiftAssignmentId);
        if (existingAttendance.Any())
            throw new InvalidOperationException("You have already checked in for this shift.");

        // 3. Get the shift and branch for validation
        var shift = await _shiftRepository.GetByIdAsync(assignment.ShiftId);
        if (shift == null)
            throw new InvalidOperationException("Associated shift not found.");

        var now = GetVietnamNow();
        var scheduledStart = assignment.WorkDate.Date + shift.StartTime;
        var scheduledEnd = assignment.WorkDate.Date + shift.EndTime;
        if (shift.EndTime <= shift.StartTime)
            scheduledEnd = scheduledEnd.AddDays(1);

        var checkInOpensAt = scheduledStart.AddMinutes(-CheckInOpenMinutes);
        if (now < checkInOpensAt)
            throw new InvalidOperationException(
                $"Chưa đến giờ vào ca. Bạn có thể chấm công từ {checkInOpensAt:HH:mm}, " +
                $"sớm hơn giờ bắt đầu {CheckInOpenMinutes} phút.");

        if (now > scheduledEnd)
            throw new InvalidOperationException(
                "Ca làm việc đã kết thúc, không thể chấm công vào ca.");

        var branch = await _branchRepository.GetByIdAsync(shift.BranchId);
        if (branch == null)
            throw new InvalidOperationException("Associated branch not found.");

        // 4. GPS Validation (Haversine)
        if (branch.Latitude.HasValue && branch.Longitude.HasValue)
        {
            var distance = CalculateHaversineDistance(
                dto.Latitude, dto.Longitude,
                branch.Latitude.Value, branch.Longitude.Value);

            if (distance > MaxDistanceMeters)
                throw new InvalidOperationException(
                    $"You are too far from the store ({distance:F0}m). " +
                    $"Maximum allowed distance is {MaxDistanceMeters}m.");
        }

        // 5. WiFi BSSID Validation
        if (!string.IsNullOrEmpty(branch.AllowedWifiBssid) && !string.IsNullOrEmpty(dto.WifiBssid))
        {
            var allowedBssids = branch.AllowedWifiBssid
                .Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
                .Select(b => b.ToUpperInvariant());

            if (!allowedBssids.Contains(dto.WifiBssid.Trim().ToUpperInvariant()))
                throw new InvalidOperationException(
                    "WiFi network verification failed. Please connect to the store's WiFi network.");
        }

        // 6. A 15-minute grace period is still considered on time.
        var isLate = now > scheduledStart.AddMinutes(LateGraceMinutes);
        var lateMinutes = isLate
            ? (int)Math.Max(0, Math.Floor((now - scheduledStart).TotalMinutes))
            : 0;
        var checkInStatus = isLate ? AttendanceStatus.Late : AttendanceStatus.Ontime;

        // 7. Create attendance record
        var attendance = new Attendance
        {
            ShiftAssignmentId = dto.ShiftAssignmentId,
            CheckInTime = now,
            CheckInStatus = checkInStatus,
            CheckInLatitude = dto.Latitude,
            CheckInLongitude = dto.Longitude,
            CheckInWifiBssid = dto.WifiBssid,
            LateMinutes = lateMinutes,
            EarlyMinutes = 0
        };

        var created = await _attendanceRepository.AddAsync(attendance);
        await _unitOfWork.SaveChangesAsync();

        var employee = await _userRepository.GetByIdAsync(employeeId);

        return MapToDto(created, assignment, shift, employee);
    }

    public async Task<AttendanceDto> CheckOutAsync(int employeeId, CheckOutRequestDto dto)
    {
        // 1. Validate the attendance record
        var attendance = await _attendanceRepository.GetByIdAsync(dto.AttendanceId);
        if (attendance == null)
            throw new InvalidOperationException("Attendance record not found.");

        if (attendance.CheckOutTime.HasValue)
            throw new InvalidOperationException("You have already checked out for this shift.");

        // 2. Validate ownership
        var assignment = await _assignmentRepository.GetByIdAsync(attendance.ShiftAssignmentId);
        if (assignment == null || assignment.EmployeeId != employeeId)
            throw new InvalidOperationException("This attendance record does not belong to you.");

        // 3. Get shift and branch
        var shift = await _shiftRepository.GetByIdAsync(assignment.ShiftId);
        if (shift == null)
            throw new InvalidOperationException("Associated shift not found.");

        var branch = await _branchRepository.GetByIdAsync(shift.BranchId);

        // 4. GPS Validation for check-out
        if (branch != null && branch.Latitude.HasValue && branch.Longitude.HasValue)
        {
            var distance = CalculateHaversineDistance(
                dto.Latitude, dto.Longitude,
                branch.Latitude.Value, branch.Longitude.Value);

            if (distance > MaxDistanceMeters)
                throw new InvalidOperationException(
                    $"You are too far from the store ({distance:F0}m). " +
                    $"Maximum allowed distance is {MaxDistanceMeters}m.");
        }

        // 5. Calculate early leave minutes
        var now = GetVietnamNow();
        var scheduledEnd = assignment.WorkDate.Date + shift.EndTime;
        if (shift.EndTime <= shift.StartTime)
            scheduledEnd = scheduledEnd.AddDays(1);
        var isEarlyLeave = now < scheduledEnd;
        var earlyMinutes = isEarlyLeave
            ? Math.Max(1, (int)Math.Ceiling((scheduledEnd - now).TotalMinutes))
            : 0;
        var checkOutStatus = isEarlyLeave
            ? AttendanceStatus.EarlyLeave
            : AttendanceStatus.Ontime;

        // 6. Update attendance record
        attendance.CheckOutTime = now;
        attendance.CheckOutStatus = checkOutStatus;
        attendance.CheckOutLatitude = dto.Latitude;
        attendance.CheckOutLongitude = dto.Longitude;
        attendance.EarlyMinutes = earlyMinutes;

        _attendanceRepository.Update(attendance);
        await _unitOfWork.SaveChangesAsync();

        var employee = await _userRepository.GetByIdAsync(employeeId);

        return MapToDto(attendance, assignment, shift, employee);
    }

    public async Task<AttendanceDto?> GetTodayAttendanceAsync(int employeeId)
    {
        var today = GetVietnamNow().Date;
        var assignments = (await _assignmentRepository.GetAsync(
            assignment => assignment.EmployeeId == employeeId
                          && assignment.WorkDate.Date == today
                          && assignment.Status != ShiftStatus.Cancelled)).ToList();
        if (assignments.Count == 0) return null;

        var assignmentIds = assignments.Select(item => item.AssignmentId).ToHashSet();
        var records = (await _attendanceRepository.GetAsync(
            attendance => assignmentIds.Contains(attendance.ShiftAssignmentId)))
            .OrderByDescending(item => item.CheckInTime)
            .ToList();
        var record = records.FirstOrDefault(item => !item.CheckOutTime.HasValue)
                     ?? records.FirstOrDefault();
        if (record == null) return null;

        var assignment = assignments.First(item =>
            item.AssignmentId == record.ShiftAssignmentId);
        var shift = await _shiftRepository.GetByIdAsync(assignment.ShiftId);
        if (shift == null) return null;
        var employee = await _userRepository.GetByIdAsync(employeeId);
        return MapToDto(record, assignment, shift, employee);
    }

    public async Task<IEnumerable<AttendanceReportDto>> GetAttendanceReportAsync(
        int branchId, DateTime startDate, DateTime endDate)
    {
        // Get all shifts for this branch
        var branchShifts = await _shiftRepository.GetAsync(s => s.BranchId == branchId);
        var shiftIds = branchShifts.Select(s => s.ShiftId).ToHashSet();

        // Get all assignments in date range for those shifts
        var assignments = await _assignmentRepository.GetAsync(
            a => shiftIds.Contains(a.ShiftId)
                 && a.WorkDate >= startDate.Date
                 && a.WorkDate <= endDate.Date
                 && a.Status != ShiftStatus.Cancelled);

        // Get all attendance records for those assignments
        var assignmentIds = assignments.Select(a => a.AssignmentId).ToHashSet();
        var allAttendances = await _attendanceRepository.GetAsync(
            a => assignmentIds.Contains(a.ShiftAssignmentId));

        // Get employees
        var users = await _userRepository.GetAsync(u => u.BranchId == branchId);

        // Group by employee and compute aggregates
        var report = new List<AttendanceReportDto>();

        foreach (var user in users)
        {
            var userAssignments = assignments.Where(a => a.EmployeeId == user.UserId).ToList();
            var userAttendances = allAttendances
                .Where(a => userAssignments.Any(ua => ua.AssignmentId == a.ShiftAssignmentId))
                .ToList();

            report.Add(new AttendanceReportDto
            {
                EmployeeId = user.UserId,
                EmployeeName = user.FullName,
                TotalShifts = userAssignments.Count,
                TotalPresent = userAttendances.Count(a => a.CheckInTime.HasValue),
                TotalLate = userAttendances.Count(a => a.CheckInStatus == AttendanceStatus.Late),
                TotalEarlyLeave = userAttendances.Count(a => a.CheckOutStatus == AttendanceStatus.EarlyLeave),
                TotalAbsent = userAssignments.Count - userAttendances.Count(a => a.CheckInTime.HasValue),
                TotalLateMinutes = userAttendances.Sum(a => a.LateMinutes),
                TotalEarlyMinutes = userAttendances.Sum(a => a.EarlyMinutes)
            });
        }

        return report.OrderBy(r => r.EmployeeName).ToList();
    }

    public async Task<IEnumerable<AttendanceDto>> GetAttendanceHistoryAsync(
        int employeeId, DateTime startDate, DateTime endDate)
    {
        var employee = await _userRepository.GetByIdAsync(employeeId);
        if (employee == null) return [];

        var assignments = (await _assignmentRepository.GetAsync(
            a => a.EmployeeId == employeeId
                 && a.WorkDate >= startDate.Date
                 && a.WorkDate <= endDate.Date
                 && a.Status != ShiftStatus.Cancelled)).ToList();
        var assignmentIds = assignments.Select(a => a.AssignmentId).ToHashSet();
        var attendances = await _attendanceRepository.GetAsync(
            a => assignmentIds.Contains(a.ShiftAssignmentId));
        var shifts = await _shiftRepository.GetAllAsync();

        return attendances
            .Select(attendance =>
            {
                var assignment = assignments.First(a =>
                    a.AssignmentId == attendance.ShiftAssignmentId);
                var shift = shifts.First(s => s.ShiftId == assignment.ShiftId);
                return MapToDto(attendance, assignment, shift, employee);
            })
            .OrderByDescending(item => item.WorkDate)
            .ThenByDescending(item => item.CheckInTime)
            .ToList();
    }

    public async Task<AttendanceDto> SaveManualAttendanceAsync(ManualAttendanceRequestDto dto)
    {
        var assignment = await _assignmentRepository.GetByIdAsync(dto.ShiftAssignmentId)
            ?? throw new InvalidOperationException("Không tìm thấy ca làm việc đã phân.");
        var shift = await _shiftRepository.GetByIdAsync(assignment.ShiftId)
            ?? throw new InvalidOperationException("Không tìm thấy thông tin ca làm việc.");
        var employee = await _userRepository.GetByIdAsync(assignment.EmployeeId);

        if (dto.CheckOutTime.HasValue && dto.CheckOutTime.Value <= dto.CheckInTime)
            throw new InvalidOperationException("Giờ tan ca phải sau giờ vào ca.");

        var scheduledStart = assignment.WorkDate.Date + shift.StartTime;
        var scheduledEnd = assignment.WorkDate.Date + shift.EndTime;
        if (shift.EndTime <= shift.StartTime) scheduledEnd = scheduledEnd.AddDays(1);

        var records = await _attendanceRepository.GetAsync(
            a => a.ShiftAssignmentId == dto.ShiftAssignmentId);
        var attendance = records.FirstOrDefault();
        var isNew = attendance == null;
        if (attendance == null)
        {
            attendance = new Attendance { ShiftAssignmentId = dto.ShiftAssignmentId };
            await _attendanceRepository.AddAsync(attendance);
        }

        attendance.CheckInTime = dto.CheckInTime;
        attendance.LateMinutes = (int)Math.Max(0,
            (dto.CheckInTime - scheduledStart).TotalMinutes);
        attendance.CheckInStatus = attendance.LateMinutes > 0
            ? AttendanceStatus.Late
            : AttendanceStatus.Ontime;
        attendance.CheckOutTime = dto.CheckOutTime;
        attendance.EarlyMinutes = dto.CheckOutTime.HasValue
            ? (int)Math.Max(0, (scheduledEnd - dto.CheckOutTime.Value).TotalMinutes)
            : 0;
        attendance.CheckOutStatus = dto.CheckOutTime.HasValue
            ? attendance.EarlyMinutes > 0
                ? AttendanceStatus.EarlyLeave
                : AttendanceStatus.Ontime
            : null;

        if (!isNew)
            _attendanceRepository.Update(attendance);
        await _unitOfWork.SaveChangesAsync();
        return MapToDto(attendance, assignment, shift, employee);
    }

    /// <summary>
    /// Calculates the distance in meters between two GPS coordinates using the Haversine formula.
    /// </summary>
    private static double CalculateHaversineDistance(
        double lat1, double lon1, double lat2, double lon2)
    {
        const double EarthRadiusMeters = 6_371_000.0;

        var dLat = DegreesToRadians(lat2 - lat1);
        var dLon = DegreesToRadians(lon2 - lon1);

        var a = Math.Sin(dLat / 2) * Math.Sin(dLat / 2) +
                Math.Cos(DegreesToRadians(lat1)) * Math.Cos(DegreesToRadians(lat2)) *
                Math.Sin(dLon / 2) * Math.Sin(dLon / 2);

        var c = 2 * Math.Asin(Math.Sqrt(a));

        return EarthRadiusMeters * c;
    }

    private static double DegreesToRadians(double degrees)
    {
        return degrees * Math.PI / 180.0;
    }

    private static DateTime GetVietnamNow()
    {
        foreach (var timeZoneId in new[] { "Asia/Ho_Chi_Minh", "SE Asia Standard Time" })
        {
            try
            {
                var timeZone = TimeZoneInfo.FindSystemTimeZoneById(timeZoneId);
                return TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, timeZone);
            }
            catch (TimeZoneNotFoundException)
            {
                // Try the ID used by the other operating system.
            }
            catch (InvalidTimeZoneException)
            {
                // Fall back to UTC+7 below if the installed time-zone data is invalid.
            }
        }

        return DateTime.SpecifyKind(DateTime.UtcNow.AddHours(7), DateTimeKind.Unspecified);
    }

    private static AttendanceDto MapToDto(
        Attendance attendance, ShiftAssignment assignment, Shift shift, User? employee)
    {
        return new AttendanceDto
        {
            AttendanceId = attendance.AttendanceId,
            ShiftAssignmentId = attendance.ShiftAssignmentId,
            EmployeeId = assignment.EmployeeId,
            EmployeeName = employee?.FullName,
            ShiftName = shift.ShiftName,
            WorkDate = assignment.WorkDate,
            CheckInTime = attendance.CheckInTime,
            CheckOutTime = attendance.CheckOutTime,
            CheckInStatus = attendance.CheckInStatus?.ToString(),
            CheckOutStatus = attendance.CheckOutStatus?.ToString(),
            LateMinutes = attendance.LateMinutes,
            EarlyMinutes = attendance.EarlyMinutes
        };
    }
}
