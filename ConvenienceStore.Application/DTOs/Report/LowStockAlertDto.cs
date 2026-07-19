namespace ConvenienceStore.Application.DTOs.Report
{
    public class LowStockAlertDto
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; } = string.Empty;
        public string CategoryName { get; set; } = string.Empty;
        public string LocationType { get; set; } = string.Empty; // "Warehouse" or "Branch"
        public string LocationName { get; set; } = string.Empty;
        public int CurrentStock { get; set; }
        public int MinimumThreshold { get; set; }
    }
}
