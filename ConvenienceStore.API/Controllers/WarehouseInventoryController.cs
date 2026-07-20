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
    public async Task<IActionResult> GetAll(
        [FromQuery] string? search,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 10,
        [FromQuery] bool lowStockOnly = false)
    {
        if (page < 1) page = 1;
        if (pageSize < 1) pageSize = 20;
        if (pageSize > 100) pageSize = 100;

        const int lowStockThreshold = 50;
        var result = await _service.GetAllWarehouseInventoryAsync(
            search,
            page,
            pageSize,
            lowStockOnly,
            lowStockThreshold);
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
    public async Task<IActionResult> GetLowStock([FromQuery] int threshold = 50)
    {
        if (threshold < 0)
            return BadRequest(new { Message = "Threshold cannot be negative." });

        var result = await _service.GetLowStockProductsAsync(threshold);
        return Ok(result);
    }

}
