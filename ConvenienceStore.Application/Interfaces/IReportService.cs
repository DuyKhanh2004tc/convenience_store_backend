using ConvenienceStore.Application.DTOs.Report;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ConvenienceStore.Application.Interfaces
{
    public interface IReportService
    {
        Task<ChainRevenueDto> GetChainRevenueAsync(DateTime startDate, DateTime endDate, string interval);
        Task<List<BranchPerformanceDto>> GetBranchPerformanceAsync(DateTime startDate, DateTime endDate);
        Task<List<TopSellingProductDto>> GetTopSellingProductsAsync(DateTime startDate, DateTime endDate, int top = 10);
        Task<List<SupplierPerformanceDto>> GetSupplierPerformanceAsync(DateTime startDate, DateTime endDate);
        Task<List<LowStockAlertDto>> GetLowStockAlertsAsync();
    }
}
