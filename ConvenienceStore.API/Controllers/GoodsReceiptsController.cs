using System.Security.Claims;
using ConvenienceStore.Application.DTOs.GoodsReceipt;
using ConvenienceStore.Application.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;

namespace ConvenienceStore.API.Controllers;

[Route("api/goods-receipts")]
[ApiController]
[Authorize]
public class GoodsReceiptsController : ControllerBase
{
    private readonly IGoodsReceiptService _service;

    public GoodsReceiptsController(IGoodsReceiptService service)
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
    [Authorize(Roles = "Warehouse Staff")]
    public async Task<IActionResult> Create([FromBody] CreateGoodsReceiptRequest request)
    {
        try
        {
            var userId = GetCurrentUserId();
            var result = await _service.ReceiveGoodsAsync(request, userId);
            return Ok(result);
        }
        catch (Exception ex)
        {
            return BadRequest(new { Message = ex.Message });
        }
    }

    [HttpGet]
    [Authorize(Roles = "Warehouse Staff,Chain Director,System Admin")]
    public async Task<IActionResult> GetAll()
    {
        var result = await _service.GetAllGoodsReceiptsAsync();
        return Ok(result);
    }

    [HttpGet("{id}")]
    [Authorize(Roles = "Warehouse Staff,Chain Director,System Admin")]
    public async Task<IActionResult> GetById(int id)
    {
        var result = await _service.GetGoodsReceiptByIdAsync(id);
        if (result == null) return NotFound();
        return Ok(result);
    }
}
