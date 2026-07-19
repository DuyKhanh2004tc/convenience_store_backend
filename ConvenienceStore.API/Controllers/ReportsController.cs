using ConvenienceStore.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace ConvenienceStore.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ReportsController : ControllerBase
    {
        private readonly IReportService _reportService;

        public ReportsController(IReportService reportService)
        {
            _reportService = reportService;
        }

        [HttpGet("chain-revenue")]
        public async Task<IActionResult> GetChainRevenue([FromQuery] DateTime startDate, [FromQuery] DateTime endDate, [FromQuery] string interval = "daily")
        {
            try
            {
                var result = await _reportService.GetChainRevenueAsync(startDate, endDate, interval);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }

        [HttpGet("branch-performance")]
        public async Task<IActionResult> GetBranchPerformance([FromQuery] DateTime startDate, [FromQuery] DateTime endDate)
        {
            try
            {
                var result = await _reportService.GetBranchPerformanceAsync(startDate, endDate);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }
        [HttpGet("top-products")]
        public async Task<IActionResult> GetTopSellingProducts([FromQuery] DateTime startDate, [FromQuery] DateTime endDate, [FromQuery] int top = 10)
        {
            try
            {
                var result = await _reportService.GetTopSellingProductsAsync(startDate, endDate, top);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }

        [HttpGet("supplier-performance")]
        public async Task<IActionResult> GetSupplierPerformance([FromQuery] DateTime startDate, [FromQuery] DateTime endDate)
        {
            try
            {
                var result = await _reportService.GetSupplierPerformanceAsync(startDate, endDate);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }

        [HttpGet("low-stock-alerts")]
        public async Task<IActionResult> GetLowStockAlerts()
        {
            try
            {
                var result = await _reportService.GetLowStockAlertsAsync();
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }
    }
}
