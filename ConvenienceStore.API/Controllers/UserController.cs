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
    public async Task<IActionResult> GetAllUsers()
    {
        try
        {
            var users = await _userService.GetAllUsersAsync();
            return Ok(new { StatusCode = 200, Message = "Success", Data = users });
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
}
