using ConvenienceStore.Application.DTOs.Report;
using ConvenienceStore.Application.Interfaces;
using ConvenienceStore.Domain.Entities;
using ConvenienceStore.Domain.Enums;
using ConvenienceStore.Application.Contracts.Persistence;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ConvenienceStore.Application.Services
{
    public class ReportService : IReportService
    {
        private readonly IGenericRepository<Invoice> _invoiceRepository;
        private readonly IGenericRepository<Branch> _branchRepository;
        private readonly IGenericRepository<InvoiceDetail> _invoiceDetailRepository;
        private readonly IGenericRepository<Supplier> _supplierRepository;
        private readonly IGenericRepository<PurchaseOrder> _purchaseOrderRepository;
        private readonly IGenericRepository<WarehouseInventory> _warehouseInventoryRepository;
        private readonly IGenericRepository<BranchInventory> _branchInventoryRepository;
        private readonly ISystemSettingService _settingService;

        public ReportService(
            IGenericRepository<Invoice> invoiceRepository, 
            IGenericRepository<Branch> branchRepository,
            IGenericRepository<InvoiceDetail> invoiceDetailRepository,
            IGenericRepository<Supplier> supplierRepository,
            IGenericRepository<PurchaseOrder> purchaseOrderRepository,
            IGenericRepository<WarehouseInventory> warehouseInventoryRepository,
            IGenericRepository<BranchInventory> branchInventoryRepository,
            ISystemSettingService settingService)
        {
            _invoiceRepository = invoiceRepository;
            _branchRepository = branchRepository;
            _invoiceDetailRepository = invoiceDetailRepository;
            _supplierRepository = supplierRepository;
            _purchaseOrderRepository = purchaseOrderRepository;
            _warehouseInventoryRepository = warehouseInventoryRepository;
            _branchInventoryRepository = branchInventoryRepository;
            _settingService = settingService;
        }

        public async Task<ChainRevenueDto> GetChainRevenueAsync(DateTime startDate, DateTime endDate, string interval)
        {
            var invoices = await _invoiceRepository.GetAsync(i => 
                i.InvoiceDate >= startDate && 
                i.InvoiceDate <= endDate && 
                i.Status == OrderStatus.Completed);

            var result = new ChainRevenueDto
            {
                TotalRevenuePeriod = invoices.Sum(i => i.FinalAmount),
                TotalOrdersPeriod = invoices.Count
            };

            // Group by interval
            IEnumerable<IGrouping<string, Invoice>> groupedInvoices;

            if (interval.ToLower() == "month")
            {
                groupedInvoices = invoices.GroupBy(i => i.InvoiceDate.ToString("yyyy-MM"));
            }
            else // daily by default
            {
                groupedInvoices = invoices.GroupBy(i => i.InvoiceDate.ToString("yyyy-MM-dd"));
            }

            foreach (var group in groupedInvoices.OrderBy(g => g.Key))
            {
                result.DataPoints.Add(new RevenueDataPointDto
                {
                    DateLabel = group.Key,
                    TotalRevenue = group.Sum(i => i.FinalAmount),
                    OrderCount = group.Count()
                });
            }

            return result;
        }

        public async Task<List<BranchPerformanceDto>> GetBranchPerformanceAsync(DateTime startDate, DateTime endDate)
        {
            var invoices = await _invoiceRepository.GetAsync(i => 
                i.InvoiceDate >= startDate && 
                i.InvoiceDate <= endDate && 
                i.Status == OrderStatus.Completed,
                "Branch");

            var branches = await _branchRepository.GetAllAsync();

            var branchPerformanceList = new List<BranchPerformanceDto>();

            var groupedByBranch = invoices.GroupBy(i => i.BranchId);

            foreach (var group in groupedByBranch)
            {
                var branchId = group.Key;
                var branchName = branches.FirstOrDefault(b => b.BranchId == branchId)?.BranchName ?? "Unknown";

                branchPerformanceList.Add(new BranchPerformanceDto
                {
                    BranchId = branchId,
                    BranchName = branchName,
                    TotalRevenue = group.Sum(i => i.FinalAmount),
                    TotalOrders = group.Count()
                });
            }

            // Rank them by revenue
            branchPerformanceList = branchPerformanceList.OrderByDescending(b => b.TotalRevenue).ToList();
            
            for (int i = 0; i < branchPerformanceList.Count; i++)
            {
                branchPerformanceList[i].Rank = i + 1;
            }

            return branchPerformanceList;
        }

        public async Task<List<TopSellingProductDto>> GetTopSellingProductsAsync(DateTime startDate, DateTime endDate, int top = 10)
        {
            var invoiceDetails = await _invoiceDetailRepository.GetAsync(
                id => id.Invoice.InvoiceDate >= startDate && 
                      id.Invoice.InvoiceDate <= endDate && 
                      id.Invoice.Status == OrderStatus.Completed,
                "Product", "Product.Category", "Invoice"
            );

            var topProducts = invoiceDetails
                .GroupBy(id => new { id.ProductId, id.Product.ProductName, id.Product.Category.CategoryName })
                .Select(g => new TopSellingProductDto
                {
                    ProductId = g.Key.ProductId,
                    ProductName = g.Key.ProductName,
                    CategoryName = g.Key.CategoryName,
                    TotalQuantitySold = g.Sum(id => id.Quantity),
                    TotalRevenue = g.Sum(id => id.SubTotal)
                })
                .OrderByDescending(p => p.TotalQuantitySold)
                .Take(top)
                .ToList();

            return topProducts;
        }

        public async Task<List<SupplierPerformanceDto>> GetSupplierPerformanceAsync(DateTime startDate, DateTime endDate)
        {
            var purchaseOrders = await _purchaseOrderRepository.GetAsync(
                po => po.OrderDate >= startDate && po.OrderDate <= endDate,
                "Supplier", "GoodsReceipts"
            );

            var supplierPerformance = purchaseOrders
                .GroupBy(po => new { po.SupplierId, po.Supplier.SupplierName })
                .Select(g => new SupplierPerformanceDto
                {
                    SupplierId = g.Key.SupplierId,
                    SupplierName = g.Key.SupplierName,
                    TotalOrders = g.Count(),
                    TotalValueProvided = g.Where(po => po.Status == PurchaseOrderStatus.Received).Sum(po => po.TotalAmount),
                    // Giả sử giao hàng đúng hạn là trạng thái Received và có GoodsReceipt trong vòng 3 ngày từ lúc đặt
                    OnTimeDeliveries = g.Count(po => po.Status == PurchaseOrderStatus.Received && 
                                                     po.GoodsReceipts.Any() && 
                                                     (po.GoodsReceipts.First().ReceivedDate - po.OrderDate).TotalDays <= 3),
                    LateDeliveries = g.Count(po => po.Status == PurchaseOrderStatus.Received && 
                                                   po.GoodsReceipts.Any() && 
                                                   (po.GoodsReceipts.First().ReceivedDate - po.OrderDate).TotalDays > 3)
                })
                .OrderByDescending(sp => sp.TotalValueProvided)
                .ToList();

            return supplierPerformance;
        }

        public async Task<List<LowStockAlertDto>> GetLowStockAlertsAsync()
        {
            var alerts = new List<LowStockAlertDto>();

            // Get thresholds
            var warehouseSetting = await _settingService.GetSettingAsync("WarehouseLowStockThreshold");
            int warehouseThreshold = warehouseSetting != null ? int.Parse(warehouseSetting.SettingValue) : 50;

            var branchSetting = await _settingService.GetSettingAsync("BranchLowStockThreshold");
            int branchThreshold = branchSetting != null ? int.Parse(branchSetting.SettingValue) : 10;

            // Get Warehouse Inventory
            var warehouseItems = await _warehouseInventoryRepository.GetAsync(
                wi => wi.Quantity <= warehouseThreshold, 
                "Product", "Product.Category");

            foreach (var item in warehouseItems)
            {
                alerts.Add(new LowStockAlertDto
                {
                    ProductId = item.ProductId,
                    ProductName = item.Product.ProductName,
                    CategoryName = item.Product.Category.CategoryName,
                    LocationType = "Warehouse",
                    LocationName = "Kho Tổng",
                    CurrentStock = item.Quantity,
                    MinimumThreshold = warehouseThreshold
                });
            }

            // Get Branch Inventory
            var branchItems = await _branchInventoryRepository.GetAsync(
                bi => bi.Quantity <= branchThreshold, 
                "Product", "Product.Category", "Branch");

            foreach (var item in branchItems)
            {
                alerts.Add(new LowStockAlertDto
                {
                    ProductId = item.ProductId,
                    ProductName = item.Product.ProductName,
                    CategoryName = item.Product.Category.CategoryName,
                    LocationType = "Branch",
                    LocationName = item.Branch.BranchName,
                    CurrentStock = item.Quantity,
                    MinimumThreshold = branchThreshold
                });
            }

            return alerts.OrderBy(a => a.CurrentStock).ToList();
        }
    }
}
