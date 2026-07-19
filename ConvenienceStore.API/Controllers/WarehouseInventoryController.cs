using ConvenienceStore.Application.DTOs.Inventory;
using ConvenienceStore.Application.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ConvenienceStore.API.Controllers;

[Route("api/warehouse-inventory")]
[ApiController]
[Authorize(Roles = "Warehouse Staff,Chain Director,System Admin")]
public class WarehouseInventoryController : ControllerBase
{
    private readonly IWarehouseInventoryService _service;

    public WarehouseInventoryController(IWarehouseInventoryService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] string? search, [FromQuery] int page = 1, [FromQuery] int pageSize = 20)
    {
        if (page < 1) page = 1;
        if (pageSize < 1) pageSize = 20;

        var result = await _service.GetAllWarehouseInventoryAsync(search, page, pageSize);
        return Ok(result);
    }

    [HttpGet("{productId}")]
    public async Task<IActionResult> GetByProduct(int productId)
    {
        var result = await _service.GetWarehouseInventoryByProductAsync(productId);
        if (result == null) return NotFound();
        return Ok(result);
    }

    [HttpGet("low-stock")]
    public async Task<IActionResult> GetLowStock([FromQuery] int threshold = 10)
    {
        var result = await _service.GetLowStockProductsAsync(threshold);
        return Ok(result);
    }

    [HttpPost("adjust")]
    public async Task<IActionResult> AdjustStock([FromBody] StockAdjustmentRequest request)
    {
        try
        {
            var result = await _service.AdjustStockAsync(request, User);
            return Ok(result);
        }
        catch (UnauthorizedAccessException ex)
        {
            return Forbid(ex.Message);
        }
        catch (Exception ex)
        {
            return BadRequest(new { Message = ex.Message });
        }
    }
}
