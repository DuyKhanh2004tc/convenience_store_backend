using ConvenienceStore.Application.DTOs.Product;
using ConvenienceStore.Application.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ConvenienceStore.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class ProductsController : ControllerBase
{
    private readonly IProductService _productService;

    public ProductsController(IProductService productService)
    {
        _productService = productService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] string? search, [FromQuery] int? categoryId, [FromQuery] int page = 1, [FromQuery] int pageSize = 10)
    {
        var pagedResult = await _productService.GetAllProductsAsync(search, categoryId, page, pageSize);

        // Hide cost price for Store Staff and Store Manager to protect profit margin
        if (User.IsInRole("Store Staff") || User.IsInRole("Store Manager"))
        {
            foreach (var p in pagedResult.Items)
            {
                p.CostPrice = 0;
            }
        }

        return Ok(pagedResult);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var product = await _productService.GetProductByIdAsync(id);
        if (product == null)
        {
            return NotFound(new { message = "Product not found" });
        }

        // Hide cost price for Store Staff and Store Manager to protect profit margin
        if (User.IsInRole("Store Staff") || User.IsInRole("Store Manager"))
        {
            product.CostPrice = 0;
        }

        return Ok(product);
    }

    [HttpGet("barcode/{barcode}")]
    public async Task<IActionResult> GetByBarcode(string barcode)
    {
        var product = await _productService.GetProductByBarcodeAsync(barcode);
        if (product == null)
        {
            return NotFound(new { message = "Product not found" });
        }

        // Hide cost price for Store Staff and Store Manager to protect profit margin
        if (User.IsInRole("Store Staff") || User.IsInRole("Store Manager"))
        {
            product.CostPrice = 0;
        }

        return Ok(product);
    }

    [HttpPost]
    [Authorize(Roles = "System Admin, Chain Director")]
    public async Task<IActionResult> Create([FromBody] CreateProductDto dto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        try
        {
            var created = await _productService.CreateProductAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = created.ProductId }, created);
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpPut("{id}")]
    [Authorize(Roles = "System Admin, Chain Director")]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateProductDto dto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        try
        {
            var result = await _productService.UpdateProductAsync(id, dto);
            if (!result)
            {
                return NotFound(new { message = "Product not found" });
            }

            return Ok(new { message = "Product updated successfully" });
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = "System Admin, Chain Director")]
    public async Task<IActionResult> Delete(int id)
    {
        var result = await _productService.DeleteProductAsync(id);
        if (!result)
        {
            return NotFound(new { message = "Product not found" });
        }

        return Ok(new { message = "Product deleted successfully" });
    }
}
