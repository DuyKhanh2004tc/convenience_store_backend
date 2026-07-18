using ConvenienceStore.Application.DTOs.Employee;
using ConvenienceStore.Application.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ConvenienceStore.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class EmployeesController : ControllerBase
{
    private readonly IEmployeeService _employeeService;

    public EmployeesController(IEmployeeService employeeService)
    {
        _employeeService = employeeService;
    }

    /// <summary>
    /// Create a new employee and assign to a store.
    /// </summary>
    [HttpPost]
    [Authorize(Roles = "System Admin")]
    public async Task<IActionResult> Create([FromBody] CreateEmployeeDto dto)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        try
        {
            var created = await _employeeService.CreateEmployeeAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = created.UserId }, created);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    /// <summary>
    /// Get all employees, optionally filtered by branch.
    /// </summary>
    [HttpGet]
    [Authorize(Roles = "System Admin,Store Manager")]
    public async Task<IActionResult> GetAll([FromQuery] int? branchId)
    {
        var employees = branchId.HasValue
            ? await _employeeService.GetEmployeesByBranchAsync(branchId.Value)
            : await _employeeService.GetAllEmployeesAsync();

        return Ok(employees);
    }

    /// <summary>
    /// Get a specific employee by ID.
    /// </summary>
    [HttpGet("{id}")]
    [Authorize]
    public async Task<IActionResult> GetById(int id)
    {
        var employee = await _employeeService.GetEmployeeByIdAsync(id);
        if (employee == null)
            return NotFound(new { message = "Employee not found" });

        return Ok(employee);
    }

    /// <summary>
    /// Update an employee's details.
    /// </summary>
    [HttpPut("{id}")]
    [Authorize(Roles = "System Admin,Store Manager")]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateEmployeeDto dto)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        if (User.IsInRole("Store Manager"))
        {
            var branchClaim = User.FindFirst("BranchId")?.Value;
            if (!int.TryParse(branchClaim, out var managerBranchId))
                return Forbid();

            var employee = await _employeeService.GetEmployeeByIdAsync(id);
            if (employee == null) return NotFound(new { message = "Employee not found" });
            if (employee.BranchId != managerBranchId) return Forbid();

            // A manager cannot move an employee to another branch.
            dto.BranchId = managerBranchId;
        }

        var result = await _employeeService.UpdateEmployeeAsync(id, dto);
        if (!result)
            return NotFound(new { message = "Employee not found" });

        return Ok(new { message = "Employee updated successfully" });
    }

    /// <summary>
    /// Toggle an employee's active status.
    /// </summary>
    [HttpPatch("{id}/toggle-status")]
    [Authorize(Roles = "System Admin")]
    public async Task<IActionResult> ToggleStatus(int id)
    {
        var result = await _employeeService.ToggleEmployeeStatusAsync(id);
        if (!result)
            return NotFound(new { message = "Employee not found" });

        return Ok(new { message = "Employee status toggled successfully" });
    }
}
