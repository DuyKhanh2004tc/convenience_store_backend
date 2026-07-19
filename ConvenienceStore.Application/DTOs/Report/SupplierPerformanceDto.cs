namespace ConvenienceStore.Application.DTOs.Report
{
    public class SupplierPerformanceDto
    {
        public int SupplierId { get; set; }
        public string SupplierName { get; set; } = string.Empty;
        public int TotalOrders { get; set; }
        public decimal TotalValueProvided { get; set; }
        public int OnTimeDeliveries { get; set; }
        public int LateDeliveries { get; set; }
        public double DeliveryPerformancePercentage => TotalOrders > 0 ? (double)OnTimeDeliveries / TotalOrders * 100 : 0;
    }
}
