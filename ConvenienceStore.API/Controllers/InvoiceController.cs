using ConvenienceStore.Application.DTOs.Invoice;
using ConvenienceStore.Application.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ConvenienceStore.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class InvoiceController : ControllerBase
{
    private readonly IInvoiceService _invoiceService;

    public InvoiceController(IInvoiceService invoiceService)
    {
        _invoiceService = invoiceService;
    }

    /// <summary>
    /// POS Checkout: Store Staff creates a sales invoice.
    /// Deducts BranchInventory and updates customer loyalty points (with Transaction).
    /// </summary>
    [HttpPost]
    [Authorize(Roles = "Store Staff, Store Manager")]
    public async Task<IActionResult> CreateInvoice([FromBody] CreateInvoiceDto request)
    {
        try
        {
            var userId = GetCurrentUserId();
            var invoice = await _invoiceService.CreateInvoiceAsync(userId, request);
            return StatusCode(201, new { StatusCode = 201, Message = "MSG-03: Invoice created successfully.", Data = invoice });
        }
        catch (Exception ex)
        {
            return BadRequest(new { StatusCode = 400, Message = ex.Message });
        }
    }

    /// <summary>
    /// Get all invoices with optional filters (branchId, fromDate, toDate).
    /// </summary>
    [HttpGet]
    [Authorize(Roles = "System Admin, Chain Director, Store Manager, Store Staff")]
    public async Task<IActionResult> GetAllInvoices(
        [FromQuery] int? branchId,
        [FromQuery] DateTime? fromDate,
        [FromQuery] DateTime? toDate)
    {
        try
        {
            var invoices = await _invoiceService.GetAllInvoicesAsync(branchId, fromDate, toDate);
            return Ok(new { StatusCode = 200, Message = "Success", Data = invoices });
        }
        catch (Exception ex)
        {
            return BadRequest(new { StatusCode = 400, Message = ex.Message });
        }
    }

    /// <summary>
    /// Get a single invoice by ID (for receipt reprint).
    /// </summary>
    [HttpGet("{id}")]
    [Authorize(Roles = "System Admin, Chain Director, Store Manager, Store Staff")]
    public async Task<IActionResult> GetInvoiceById(int id)
    {
        try
        {
            var invoice = await _invoiceService.GetInvoiceByIdAsync(id);
            if (invoice == null)
                return NotFound(new { StatusCode = 404, Message = "Invoice not found." });

            return Ok(new { StatusCode = 200, Message = "Success", Data = invoice });
        }
        catch (Exception ex)
        {
            return BadRequest(new { StatusCode = 400, Message = ex.Message });
        }
    }

    /// <summary>
    /// Get all invoices for a specific branch.
    /// </summary>
    [HttpGet("branch/{branchId}")]
    [Authorize(Roles = "System Admin, Chain Director, Store Manager, Store Staff")]
    public async Task<IActionResult> GetInvoicesByBranch(int branchId)
    {
        try
        {
            var invoices = await _invoiceService.GetInvoicesByBranchAsync(branchId);
            return Ok(new { StatusCode = 200, Message = "Success", Data = invoices });
        }
        catch (Exception ex)
        {
            return BadRequest(new { StatusCode = 400, Message = ex.Message });
        }
    }

    /// <summary>
    /// Get daily sales summary for a branch (used for revenue reporting).
    /// </summary>
    [HttpGet("summary/daily")]
    [Authorize(Roles = "System Admin, Chain Director, Store Manager, Store Staff")]
    public async Task<IActionResult> GetDailySalesSummary(
        [FromQuery] int branchId,
        [FromQuery] DateTime date)
    {
        try
        {
            var summary = await _invoiceService.GetDailySalesSummaryAsync(branchId, date);
            return Ok(new { StatusCode = 200, Message = "Success", Data = summary });
        }
        catch (Exception ex)
        {
            return BadRequest(new { StatusCode = 400, Message = ex.Message });
        }
    }

    /// <summary>
    /// Get period sales summary for a branch (used for charting and period reporting).
    /// </summary>
    [HttpGet("summary/period")]
    [Authorize(Roles = "System Admin, Chain Director, Store Manager, Store Staff")]
    public async Task<IActionResult> GetPeriodSalesSummary(
        [FromQuery] int branchId,
        [FromQuery] DateTime startDate,
        [FromQuery] DateTime endDate)
    {
        try
        {
            var summary = await _invoiceService.GetPeriodSalesSummaryAsync(branchId, startDate, endDate);
            return Ok(new { StatusCode = 200, Message = "Success", Data = summary });
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
