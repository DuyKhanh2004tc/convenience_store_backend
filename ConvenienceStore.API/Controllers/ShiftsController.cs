using ConvenienceStore.Application.DTOs.Shift;
using ConvenienceStore.Application.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ConvenienceStore.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ShiftsController : ControllerBase
{
    private readonly IShiftService _shiftService;
    private readonly IEmployeeService _employeeService;

    public ShiftsController(IShiftService shiftService, IEmployeeService employeeService)
    {
        _shiftService = shiftService;
        _employeeService = employeeService;
    }

    /// <summary>
    /// Create a base shift definition for a branch.
    /// </summary>
    [HttpPost]
    [Authorize(Roles = "System Admin,Store Manager")]
    public async Task<IActionResult> CreateShift([FromBody] CreateShiftDto dto)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        if (User.IsInRole("Store Manager"))
        {
            var branchClaim = User.FindFirst("BranchId")?.Value;
            if (!int.TryParse(branchClaim, out var managerBranchId)) return Forbid();
            dto.BranchId = managerBranchId;
        }

        try
        {
            var created = await _shiftService.CreateShiftAsync(dto);
            return Ok(created);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    /// <summary>
    /// Get all base shift definitions for a branch.
    /// </summary>
    [HttpGet]
    [Authorize]
    public async Task<IActionResult> GetShifts([FromQuery] int branchId)
    {
        var shifts = await _shiftService.GetShiftsByBranchAsync(branchId);
        return Ok(shifts);
    }

    [HttpPut("{id}")]
    [Authorize(Roles = "System Admin,Store Manager")]
    public async Task<IActionResult> UpdateShift(int id, [FromBody] UpdateShiftDto dto)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        if (User.IsInRole("Store Manager"))
        {
            var branchClaim = User.FindFirst("BranchId")?.Value;
            if (!int.TryParse(branchClaim, out var managerBranchId)) return Forbid();
            var branchShifts = await _shiftService.GetShiftsByBranchAsync(managerBranchId);
            if (!branchShifts.Any(shift => shift.ShiftId == id)) return Forbid();
        }

        var updated = await _shiftService.UpdateShiftAsync(id, dto);
        return updated == null
            ? NotFound(new { message = "Shift not found" })
            : Ok(updated);
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = "System Admin,Store Manager")]
    public async Task<IActionResult> DeleteShift(int id)
    {
        if (User.IsInRole("Store Manager"))
        {
            var branchClaim = User.FindFirst("BranchId")?.Value;
            if (!int.TryParse(branchClaim, out var managerBranchId)) return Forbid();
            var branchShifts = await _shiftService.GetShiftsByBranchAsync(managerBranchId);
            if (!branchShifts.Any(shift => shift.ShiftId == id)) return Forbid();
        }

        try
        {
            return await _shiftService.DeleteShiftAsync(id)
                ? NoContent()
                : NotFound(new { message = "Shift not found" });
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    /// <summary>
    /// Generate weekly/monthly shift assignments for employees.
    /// Validates for scheduling conflicts (overlapping shifts).
    /// </summary>
    [HttpPost("assign")]
    [Authorize(Roles = "System Admin,Store Manager")]
    public async Task<IActionResult> AssignShifts([FromBody] AssignShiftDto dto)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        if (User.IsInRole("Store Manager"))
        {
            var branchClaim = User.FindFirst("BranchId")?.Value;
            if (!int.TryParse(branchClaim, out var managerBranchId)) return Forbid();

            var branchShifts = await _shiftService.GetShiftsByBranchAsync(managerBranchId);
            if (!branchShifts.Any(shift => shift.ShiftId == dto.ShiftId)) return Forbid();

            foreach (var employeeId in dto.EmployeeIds)
            {
                var employee = await _employeeService.GetEmployeeByIdAsync(employeeId);
                if (employee?.BranchId != managerBranchId) return Forbid();
            }
        }

        try
        {
            var assignments = await _shiftService.AssignShiftsAsync(dto);
            return Ok(assignments);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
        catch (Exception)
        {
            return BadRequest(new
            {
                message = "Không thể lưu phân ca. Vui lòng tải lại lịch và thử lại."
            });
        }
    }

    [HttpPut("assignments/{id}")]
    [Authorize(Roles = "System Admin,Store Manager")]
    public async Task<IActionResult> UpdateAssignment(
        int id, [FromBody] UpdateShiftAssignmentDto dto)
    {
        if (User.IsInRole("Store Manager"))
        {
            var branchClaim = User.FindFirst("BranchId")?.Value;
            if (!int.TryParse(branchClaim, out var branchId)) return Forbid();
            if (!await _shiftService.AssignmentBelongsToBranchAsync(id, branchId))
                return Forbid();
            var employee = await _employeeService.GetEmployeeByIdAsync(dto.EmployeeId);
            if (employee?.BranchId != branchId) return Forbid();
        }

        var updated = await _shiftService.UpdateAssignmentAsync(id, dto.EmployeeId);
        return updated == null
            ? NotFound(new { message = "Assignment or employee not found" })
            : Ok(updated);
    }

    [HttpDelete("assignments/{id}")]
    [Authorize(Roles = "System Admin,Store Manager")]
    public async Task<IActionResult> DeleteAssignment(int id)
    {
        if (User.IsInRole("Store Manager"))
        {
            var branchClaim = User.FindFirst("BranchId")?.Value;
            if (!int.TryParse(branchClaim, out var branchId)) return Forbid();
            if (!await _shiftService.AssignmentBelongsToBranchAsync(id, branchId))
                return Forbid();
        }

        return await _shiftService.DeleteAssignmentAsync(id)
            ? NoContent()
            : NotFound(new { message = "Assignment not found" });
    }

    /// <summary>
    /// Get the authenticated staff member's personal schedule.
    /// </summary>
    [HttpGet("my-schedule")]
    [Authorize]
    public async Task<IActionResult> GetMySchedule([FromQuery] DateTime? startDate, [FromQuery] DateTime? endDate)
    {
        var userId = GetCurrentUserId();
        if (userId == null) return Unauthorized();

        var schedule = await _shiftService.GetMyScheduleAsync(userId.Value, startDate, endDate);
        return Ok(schedule);
    }

    /// <summary>
    /// Get the full store calendar for a branch (Manager/Admin view).
    /// </summary>
    [HttpGet("store-schedule")]
    [Authorize(Roles = "System Admin,Store Manager")]
    public async Task<IActionResult> GetStoreSchedule([FromQuery] int branchId, [FromQuery] DateTime? startDate, [FromQuery] DateTime? endDate)
    {
        var schedule = await _shiftService.GetStoreScheduleAsync(branchId, startDate, endDate);
        return Ok(schedule);
    }

    private int? GetCurrentUserId()
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value
                          ?? User.FindFirst("UserId")?.Value;
        return int.TryParse(userIdClaim, out var userId) ? userId : null;
    }
}
