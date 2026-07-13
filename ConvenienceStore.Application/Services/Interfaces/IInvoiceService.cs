using ConvenienceStore.Application.DTOs.Invoice;

namespace ConvenienceStore.Application.Services.Interfaces;

public interface IInvoiceService
{
    /// <summary>
    /// POS Checkout: Creates a new sales invoice.
    /// Uses Transaction to: save invoice → deduct BranchInventory → update loyalty points.
    /// </summary>
    Task<InvoiceDto> CreateInvoiceAsync(int staffUserId, CreateInvoiceDto request);

    /// <summary>
    /// Get all invoices with optional filters.
    /// </summary>
    Task<IEnumerable<InvoiceDto>> GetAllInvoicesAsync(int? branchId = null, DateTime? fromDate = null, DateTime? toDate = null);

    /// <summary>
    /// Get a single invoice by ID (for receipt reprint).
    /// </summary>
    Task<InvoiceDto?> GetInvoiceByIdAsync(int invoiceId);

    /// <summary>
    /// Get all invoices for a specific branch.
    /// </summary>
    Task<IEnumerable<InvoiceDto>> GetInvoicesByBranchAsync(int branchId);

    /// <summary>
    /// Get daily sales summary for a branch.
    /// </summary>
    Task<DailySalesSummaryDto> GetDailySalesSummaryAsync(int branchId, DateTime date);
}

/// <summary>
/// Summary DTO for daily revenue reporting.
/// </summary>
public class DailySalesSummaryDto
{
    public int BranchId { get; set; }
    public string BranchName { get; set; } = string.Empty;
    public DateTime Date { get; set; }
    public int TotalInvoices { get; set; }
    public decimal TotalRevenue { get; set; }
    public decimal TotalDiscount { get; set; }
    public decimal NetRevenue { get; set; }
}
