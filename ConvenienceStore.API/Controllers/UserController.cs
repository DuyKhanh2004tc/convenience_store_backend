using ConvenienceStore.Application.DTOs.Common;
using ConvenienceStore.Application.DTOs.User;
using ConvenienceStore.Application.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ConvenienceStore.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = "System Admin, Chain Director")]
public class UserController : ControllerBase
{
    private readonly IUserService _userService;

    public UserController(IUserService userService)
    {
        _userService = userService;
    }

    [HttpGet]
    public async Task<IActionResult> GetUsers([FromQuery] QueryParameters query)
    {
        try
        {
            var pagedResult = await _userService.GetUsersAsync(query);
            return Ok(new { StatusCode = 200, Message = "Success", Data = pagedResult });
        }
        catch (Exception ex)
        {
            return BadRequest(new { StatusCode = 400, Message = ex.Message });
        }
    }

    [HttpGet("stats")]
    public async Task<IActionResult> GetUserStats()
    {
        try
        {
            var stats = await _userService.GetUserStatsAsync();
            return Ok(new { StatusCode = 200, Message = "Success", Data = stats });
        }
        catch (Exception ex)
        {
            return BadRequest(new { StatusCode = 400, Message = ex.Message });
        }
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetUserById(int id)
    {
        try
        {
            var user = await _userService.GetUserByIdAsync(id);
            if (user == null)
                return NotFound(new { StatusCode = 404, Message = "User not found" });

            return Ok(new { StatusCode = 200, Message = "Success", Data = user });
        }
        catch (Exception ex)
        {
            return BadRequest(new { StatusCode = 400, Message = ex.Message });
        }
    }

    [HttpPost]
    public async Task<IActionResult> CreateUser([FromBody] CreateUserDto request)
    {
        try
        {
            var user = await _userService.CreateUserAsync(request);
            return StatusCode(201, new { StatusCode = 201, Message = "MSG-03: Record created successfully.", Data = user });
        }
        catch (Exception ex)
        {
            return BadRequest(new { StatusCode = 400, Message = ex.Message });
        }
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateUser(int id, [FromBody] UpdateUserDto request)
    {
        try
        {
            var user = await _userService.UpdateUserAsync(id, request);
            return Ok(new { StatusCode = 200, Message = "MSG-04: Changes have been saved successfully.", Data = user });
        }
        catch (Exception ex)
        {
            return BadRequest(new { StatusCode = 400, Message = ex.Message });
        }
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeactivateUser(int id)
    {
        try
        {
            await _userService.DeactivateUserAsync(id);
            return Ok(new { StatusCode = 200, Message = "MSG-05: Record has been deactivated successfully." });
        }
        catch (Exception ex)
        {
            return BadRequest(new { StatusCode = 400, Message = ex.Message });
        }
    }

    [HttpPut("{id}/activate")]
    public async Task<IActionResult> ActivateUser(int id)
    {
        try
        {
            await _userService.ActivateUserAsync(id);
            return Ok(new { StatusCode = 200, Message = "Record has been activated successfully." });
        }
        catch (Exception ex)
        {
            return BadRequest(new { StatusCode = 400, Message = ex.Message });
        }
    }

    [HttpPost("{id}/reset-password")]
    public async Task<IActionResult> ResetUserPassword(int id)
    {
        try
        {
            var newPassword = await _userService.ResetUserPasswordAsync(id);
            return Ok(new { StatusCode = 200, Message = "Password has been reset successfully.", Data = new { newPassword } });
        }
        catch (Exception ex)
        {
            return BadRequest(new { StatusCode = 400, Message = ex.Message });
        }
    }
}
