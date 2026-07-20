using System.Security.Claims;
using ConvenienceStore.Application.DTOs.PurchaseOrder;
using ConvenienceStore.Application.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;

namespace ConvenienceStore.API.Controllers;

[Route("api/purchase-orders")]
[ApiController]
[Authorize]
public class PurchaseOrdersController : ControllerBase
{
    private readonly IPurchaseOrderService _service;

    public PurchaseOrdersController(IPurchaseOrderService service)
    {
        _service = service;
    }

    private int GetCurrentUserId()
    {
        var userIdString = User.FindFirstValue(JwtRegisteredClaimNames.Sub) ?? User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (int.TryParse(userIdString, out var userId))
            return userId;
        throw new UnauthorizedAccessException("Invalid user token.");
    }

    [HttpPost]
    [Authorize(Roles = "Chain Director")]
    public async Task<IActionResult> Create([FromBody] CreatePurchaseOrderRequest request)
    {
        try
        {
            var userId = GetCurrentUserId();
            var result = await _service.CreatePurchaseOrderAsync(request, userId);
            return Ok(result);
        }
        catch (Exception ex)
        {
            return BadRequest(new { Message = ex.Message });
        }
    }

    [HttpGet]
    [Authorize(Roles = "Warehouse Staff,Chain Director,System Admin")]
    public async Task<IActionResult> GetAll(
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 10,
        [FromQuery] string? search = null,
        [FromQuery] string? status = null)
    {
        try
        {
            page = Math.Max(1, page);
            pageSize = Math.Clamp(pageSize, 1, 100);
            var result = await _service.GetPurchaseOrdersAsync(page, pageSize, search, status);
            return Ok(result);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { Message = ex.Message });
        }
    }

    [HttpGet("{id}")]
    [Authorize(Roles = "Warehouse Staff,Chain Director,System Admin")]
    public async Task<IActionResult> GetById(int id)
    {
        var result = await _service.GetPurchaseOrderByIdAsync(id);
        if (result == null) return NotFound();
        return Ok(result);
    }

    [HttpPut("{id}/cancel")]
    [Authorize(Roles = "Chain Director")]
    public async Task<IActionResult> Cancel(int id)
    {
        try
        {
            var result = await _service.CancelPurchaseOrderAsync(id);
            return Ok(result);
        }
        catch (Exception ex)
        {
            return BadRequest(new { Message = ex.Message });
        }
    }
}
