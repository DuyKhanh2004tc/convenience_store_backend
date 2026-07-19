using ConvenienceStore.Application.DTOs.BranchOrder;
using ConvenienceStore.Application.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ConvenienceStore.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class BranchOrderController : ControllerBase
{
    private readonly IBranchOrderService _branchOrderService;

    public BranchOrderController(IBranchOrderService branchOrderService)
    {
        _branchOrderService = branchOrderService;
    }

    /// <summary>
    /// Store Manager creates a new internal order from their branch to the warehouse.
    /// </summary>
    [HttpPost]
    [Authorize(Roles = "Store Manager")]
    public async Task<IActionResult> CreateOrder([FromBody] CreateBranchOrderDto request)
    {
        try
        {
            var userId = GetCurrentUserId();
            var order = await _branchOrderService.CreateOrderAsync(userId, request);
            return StatusCode(201, new { StatusCode = 201, Message = "MSG-03: Branch order created successfully.", Data = order });
        }
        catch (Exception ex)
        {
            return BadRequest(new { StatusCode = 400, Message = ex.Message });
        }
    }

    /// <summary>
    /// Get all orders. Supports optional filtering by branchId and/or status.
    /// </summary>
    [HttpGet]
    [Authorize(Roles = "System Admin, Chain Director, Warehouse Staff, Store Manager")]
    public async Task<IActionResult> GetAllOrders([FromQuery] int? branchId, [FromQuery] string? status)
    {
        try
        {
            var orders = await _branchOrderService.GetAllOrdersAsync(branchId, status);
            return Ok(new { StatusCode = 200, Message = "Success", Data = orders });
        }
        catch (Exception ex)
        {
            return BadRequest(new { StatusCode = 400, Message = ex.Message });
        }
    }

    /// <summary>
    /// Get a single order by ID.
    /// </summary>
    [HttpGet("{id}")]
    [Authorize(Roles = "System Admin, Chain Director, Warehouse Staff, Store Manager")]
    public async Task<IActionResult> GetOrderById(int id)
    {
        try
        {
            var order = await _branchOrderService.GetOrderByIdAsync(id);
            if (order == null)
                return NotFound(new { StatusCode = 404, Message = "Order not found." });

            return Ok(new { StatusCode = 200, Message = "Success", Data = order });
        }
        catch (Exception ex)
        {
            return BadRequest(new { StatusCode = 400, Message = ex.Message });
        }
    }

    /// <summary>
    /// Get all orders for a specific branch.
    /// </summary>
    [HttpGet("branch/{branchId}")]
    [Authorize(Roles = "System Admin, Chain Director, Warehouse Staff, Store Manager")]
    public async Task<IActionResult> GetOrdersByBranch(int branchId)
    {
        try
        {
            var orders = await _branchOrderService.GetOrdersByBranchAsync(branchId);
            return Ok(new { StatusCode = 200, Message = "Success", Data = orders });
        }
        catch (Exception ex)
        {
            return BadRequest(new { StatusCode = 400, Message = ex.Message });
        }
    }

    /// <summary>
    /// Warehouse Staff approves a pending order.
    /// Deducts from WarehouseInventory and adds to BranchInventory (with Transaction).
    /// </summary>
    [HttpPut("{id}/approve")]
    [Authorize(Roles = "Warehouse Staff")]
    public async Task<IActionResult> ApproveOrder(int id)
    {
        try
        {
            var order = await _branchOrderService.ApproveOrderAsync(id);
            return Ok(new { StatusCode = 200, Message = "MSG-04: Order has been approved and warehouse inventory updated.", Data = order });
        }
        catch (Exception ex)
        {
            return BadRequest(new { StatusCode = 400, Message = ex.Message });
        }
    }

    /// <summary>
    /// Store Manager confirms they received the order.
    /// Adds to BranchInventory (with Transaction).
    /// </summary>
    [HttpPut("{id}/confirm-received")]
    [Authorize(Roles = "Store Manager")]
    public async Task<IActionResult> ConfirmReceived(int id)
    {
        try
        {
            var order = await _branchOrderService.ConfirmReceivedAsync(id);
            return Ok(new { StatusCode = 200, Message = "MSG-06: Order has been confirmed and branch inventory updated.", Data = order });
        }
        catch (Exception ex)
        {
            return BadRequest(new { StatusCode = 400, Message = ex.Message });
        }
    }

    /// <summary>
    /// Cancel a pending order.
    /// </summary>
    [HttpPut("{id}/cancel")]
    [Authorize(Roles = "Warehouse Staff, Store Manager")]
    public async Task<IActionResult> CancelOrder(int id)
    {
        try
        {
            var order = await _branchOrderService.CancelOrderAsync(id);
            return Ok(new { StatusCode = 200, Message = "MSG-05: Order has been cancelled.", Data = order });
        }
        catch (Exception ex)
        {
            return BadRequest(new { StatusCode = 400, Message = ex.Message });
        }
    }

    // ===== PRIVATE HELPER =====

    private int GetCurrentUserId()
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value
                          ?? User.FindFirst("UserId")?.Value;

        if (string.IsNullOrEmpty(userIdClaim) || !int.TryParse(userIdClaim, out var userId))
            throw new Exception("Unable to identify current user.");

        return userId;
    }
}
