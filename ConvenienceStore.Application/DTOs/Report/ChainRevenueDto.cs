namespace ConvenienceStore.Application.DTOs.Report
{
    public class RevenueDataPointDto
    {
        public string DateLabel { get; set; } = string.Empty;
        public decimal TotalRevenue { get; set; }
        public int OrderCount { get; set; }
    }

    public class ChainRevenueDto
    {
        public decimal TotalRevenuePeriod { get; set; }
        public int TotalOrdersPeriod { get; set; }
        public List<RevenueDataPointDto> DataPoints { get; set; } = new List<RevenueDataPointDto>();
    }
}
