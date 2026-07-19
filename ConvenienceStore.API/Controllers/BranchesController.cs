using ConvenienceStore.Application.DTOs.Branch;
using ConvenienceStore.Application.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ConvenienceStore.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class BranchesController : ControllerBase
{
    private readonly IBranchService _branchService;

    public BranchesController(IBranchService branchService)
    {
        _branchService = branchService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var branches = await _branchService.GetAllBranchesAsync();
        return Ok(branches);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var branch = await _branchService.GetBranchByIdAsync(id);
        if (branch == null)
        {
            return NotFound(new { message = "Branch not found" });
        }
        return Ok(branch);
    }

    [HttpPost]
    [Authorize(Roles = "System Admin, Chain Director")]
    public async Task<IActionResult> Create([FromBody] CreateBranchDto dto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var created = await _branchService.CreateBranchAsync(dto);
        return CreatedAtAction(nameof(GetById), new { id = created.BranchId }, created);
    }

    [HttpPut("{id}")]
    [Authorize(Roles = "System Admin, Chain Director")]
    public async Task<IActionResult> Update(int id, [FromBody] CreateBranchDto dto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var result = await _branchService.UpdateBranchAsync(id, dto);
        if (!result)
        {
            return NotFound(new { message = "Branch not found" });
        }

        return Ok(new { message = "Branch updated successfully" });
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = "System Admin, Chain Director")]
    public async Task<IActionResult> Delete(int id)
    {
        var result = await _branchService.DeleteBranchAsync(id);
        if (!result)
        {
            return NotFound(new { message = "Branch not found" });
        }

        return Ok(new { message = "Branch deleted successfully" });
    }
}
