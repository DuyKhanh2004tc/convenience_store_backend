namespace ConvenienceStore.Application.DTOs.Report
{
    public class BranchPerformanceDto
    {
        public int BranchId { get; set; }
        public string BranchName { get; set; } = string.Empty;
        public decimal TotalRevenue { get; set; }
        public int TotalOrders { get; set; }
        public int Rank { get; set; }
    }
}
