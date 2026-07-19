using ConvenienceStore.Application.DTOs.Supplier;
using ConvenienceStore.Application.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ConvenienceStore.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class SuppliersController : ControllerBase
{
    private readonly ISupplierService _supplierService;

    public SuppliersController(ISupplierService supplierService)
    {
        _supplierService = supplierService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
    {
        var pagedResult = await _supplierService.GetAllSuppliersAsync(page, pageSize);
        return Ok(pagedResult);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var supplier = await _supplierService.GetSupplierByIdAsync(id);
        if (supplier == null)
        {
            return NotFound(new { message = "Supplier not found" });
        }
        return Ok(supplier);
    }

    [HttpPost]
    [Authorize(Roles = "System Admin, Chain Director")]
    public async Task<IActionResult> Create([FromBody] CreateSupplierDto dto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var created = await _supplierService.CreateSupplierAsync(dto);
        return CreatedAtAction(nameof(GetById), new { id = created.SupplierId }, created);
    }

    [HttpPut("{id}")]
    [Authorize(Roles = "System Admin, Chain Director")]
    public async Task<IActionResult> Update(int id, [FromBody] CreateSupplierDto dto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var result = await _supplierService.UpdateSupplierAsync(id, dto);
        if (!result)
        {
            return NotFound(new { message = "Supplier not found" });
        }

        return Ok(new { message = "Supplier updated successfully" });
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = "System Admin, Chain Director")]
    public async Task<IActionResult> Delete(int id)
    {
        var result = await _supplierService.DeleteSupplierAsync(id);
        if (!result)
        {
            return NotFound(new { message = "Supplier not found" });
        }

        return Ok(new { message = "Supplier deleted successfully" });
    }
}
