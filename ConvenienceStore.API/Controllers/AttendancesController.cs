using ConvenienceStore.Application.DTOs.Attendance;
using ConvenienceStore.Application.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ConvenienceStore.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AttendancesController : ControllerBase
{
    private readonly IAttendanceService _attendanceService;
    private readonly IShiftService _shiftService;

    public AttendancesController(
        IAttendanceService attendanceService,
        IShiftService shiftService)
    {
        _attendanceService = attendanceService;
        _shiftService = shiftService;
    }

    /// <summary>
    /// Staff check-in with GPS and WiFi validation.
    /// </summary>
    [HttpPost("check-in")]
    [Authorize]
    public async Task<IActionResult> CheckIn([FromBody] CheckInRequestDto dto)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        var userId = GetCurrentUserId();
        if (userId == null) return Unauthorized();

        try
        {
            var result = await _attendanceService.CheckInAsync(userId.Value, dto);
            return Ok(result);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    /// <summary>
    /// Staff check-out with GPS validation.
    /// </summary>
    [HttpPost("check-out")]
    [Authorize]
    public async Task<IActionResult> CheckOut([FromBody] CheckOutRequestDto dto)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        var userId = GetCurrentUserId();
        if (userId == null) return Unauthorized();

        try
        {
            var result = await _attendanceService.CheckOutAsync(userId.Value, dto);
            return Ok(result);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    /// <summary>
    /// Get attendance report for a branch within a date range.
    /// </summary>
    [HttpGet("report")]
    [Authorize(Roles = "System Admin,Store Manager")]
    public async Task<IActionResult> GetReport(
        [FromQuery] int branchId,
        [FromQuery] DateTime startDate,
        [FromQuery] DateTime endDate)
    {
        var report = await _attendanceService.GetAttendanceReportAsync(branchId, startDate, endDate);
        return Ok(report);
    }

    [HttpGet("history")]
    [Authorize(Roles = "System Admin,Store Manager")]
    public async Task<IActionResult> GetHistory(
        [FromQuery] int employeeId,
        [FromQuery] DateTime startDate,
        [FromQuery] DateTime endDate)
    {
        if (User.IsInRole("Store Manager"))
        {
            var branchClaim = User.FindFirst("BranchId")?.Value;
            if (!int.TryParse(branchClaim, out var managerBranchId)) return Forbid();

            var branchReport = await _attendanceService.GetAttendanceReportAsync(
                managerBranchId, startDate, endDate);
            if (!branchReport.Any(item => item.EmployeeId == employeeId)) return Forbid();
        }

        var history = await _attendanceService.GetAttendanceHistoryAsync(
            employeeId, startDate, endDate);
        return Ok(history);
    }

    [HttpPost("manual")]
    [Authorize(Roles = "System Admin,Store Manager")]
    public async Task<IActionResult> SaveManualAttendance(
        [FromBody] ManualAttendanceRequestDto dto)
    {
        if (User.IsInRole("Store Manager"))
        {
            var branchClaim = User.FindFirst("BranchId")?.Value;
            if (!int.TryParse(branchClaim, out var managerBranchId)) return Forbid();

            var branchSchedule = await _shiftService.GetStoreScheduleAsync(
                managerBranchId, dto.CheckInTime.Date, dto.CheckInTime.Date);
            if (!branchSchedule.Any(item => item.AssignmentId == dto.ShiftAssignmentId))
                return Forbid();
        }

        try
        {
            return Ok(await _attendanceService.SaveManualAttendanceAsync(dto));
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    private int? GetCurrentUserId()
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value
                          ?? User.FindFirst("UserId")?.Value;
        return int.TryParse(userIdClaim, out var userId) ? userId : null;
    }
}
