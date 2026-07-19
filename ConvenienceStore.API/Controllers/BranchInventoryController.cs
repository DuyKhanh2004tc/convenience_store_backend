using ConvenienceStore.Application.DTOs.Inventory;
using ConvenienceStore.Application.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ConvenienceStore.API.Controllers;

[Route("api/branches/{branchId}/inventory")]
[ApiController]
[Authorize(Roles = "Store Manager,Store Staff,Chain Director,System Admin")]
public class BranchInventoryController : ControllerBase
{
    private readonly IBranchInventoryService _service;

    public BranchInventoryController(IBranchInventoryService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<IActionResult> GetInventory(int branchId, [FromQuery] string? search, [FromQuery] int page = 1, [FromQuery] int pageSize = 20)
    {
        try
        {
            if (page < 1) page = 1;
            if (pageSize < 1) pageSize = 20;

            var result = await _service.GetBranchInventoryAsync(branchId, search, page, pageSize, User);
            return Ok(result);
        }
        catch (UnauthorizedAccessException ex)
        {
            return StatusCode(StatusCodes.Status403Forbidden, new { Message = ex.Message });
        }
        catch (Exception ex)
        {
            return BadRequest(new { Message = ex.Message });
        }
    }

    [HttpGet("{productId}")]
    public async Task<IActionResult> GetByProduct(int branchId, int productId)
    {
        try
        {
            var result = await _service.GetBranchInventoryByProductAsync(branchId, productId, User);
            if (result == null) return NotFound();
            return Ok(result);
        }
        catch (UnauthorizedAccessException ex)
        {
            return StatusCode(StatusCodes.Status403Forbidden, new { Message = ex.Message });
        }
    }

    [HttpGet("low-stock")]
    public async Task<IActionResult> GetLowStock(int branchId, [FromQuery] int threshold = 10)
    {
        try
        {
            var result = await _service.GetLowStockByBranchAsync(branchId, threshold, User);
            return Ok(result);
        }
        catch (UnauthorizedAccessException ex)
        {
            return StatusCode(StatusCodes.Status403Forbidden, new { Message = ex.Message });
        }
    }

    [HttpPost("adjust")]
    [Authorize(Roles = "Store Manager,Chain Director,System Admin")]
    public async Task<IActionResult> AdjustStock(int branchId, [FromBody] StockAdjustmentRequest request)
    {
        try
        {
            var result = await _service.AdjustStockAsync(branchId, request, User);
            return Ok(result);
        }
        catch (UnauthorizedAccessException ex)
        {
            return StatusCode(StatusCodes.Status403Forbidden, new { Message = ex.Message });
        }
        catch (Exception ex)
        {
            return BadRequest(new { Message = ex.Message });
        }
    }
}
