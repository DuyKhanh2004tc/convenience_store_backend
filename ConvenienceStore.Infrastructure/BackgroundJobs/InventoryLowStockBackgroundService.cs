using ConvenienceStore.Application.Services.Interfaces;
using ConvenienceStore.Application.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace ConvenienceStore.Infrastructure.BackgroundJobs;

public class InventoryLowStockBackgroundService : BackgroundService
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<InventoryLowStockBackgroundService> _logger;
    private readonly TimeSpan _checkInterval = TimeSpan.FromMinutes(60);

    public InventoryLowStockBackgroundService(
        IServiceProvider serviceProvider,
        ILogger<InventoryLowStockBackgroundService> logger)
    {
        _serviceProvider = serviceProvider;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("InventoryLowStockBackgroundService is starting.");

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                await CheckLowStockAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while checking low stock.");
            }

            await Task.Delay(_checkInterval, stoppingToken);
        }

        _logger.LogInformation("InventoryLowStockBackgroundService is stopping.");
    }

    private async Task CheckLowStockAsync()
    {
        using var scope = _serviceProvider.CreateScope();
        var warehouseService = scope.ServiceProvider.GetRequiredService<IWarehouseInventoryService>();
        var branchInventoryService = scope.ServiceProvider.GetRequiredService<IBranchInventoryService>();
        var notificationService = scope.ServiceProvider.GetRequiredService<INotificationService>();
        var settingService = scope.ServiceProvider.GetRequiredService<ISystemSettingService>();

        _logger.LogInformation("Checking warehouse and branch low stock...");

        // Get thresholds
        var warehouseSetting = await settingService.GetSettingAsync("WarehouseLowStockThreshold");
        int warehouseThreshold = warehouseSetting != null ? int.Parse(warehouseSetting.SettingValue) : 50;

        var branchSetting = await settingService.GetSettingAsync("BranchLowStockThreshold");
        int branchThreshold = branchSetting != null ? int.Parse(branchSetting.SettingValue) : 10;
        
        // 1. Check Warehouse Low Stock
        var lowStockProducts = await warehouseService.GetLowStockProductsAsync(warehouseThreshold);
        foreach (var product in lowStockProducts)
        {
            var message =
                $"Sản phẩm {product.ProductName} sắp hết hàng trong kho tổng " +
                $"(còn lại: {product.Quantity}, ngưỡng cảnh báo: {warehouseThreshold}). " +
                "Vui lòng kiểm tra và nhập thêm hàng.";

            _logger.LogWarning("{LowStockMessage}", message);
            await notificationService.CreateAlertForRolesAsync(
                new List<string> { "Chain Director", "Warehouse Staff", "System Admin" },
                "Cảnh báo Tồn kho Kho Tổng",
                message
            );
        }

        // 2. Check Branch Low Stock
        var lowStockBranchProducts = await branchInventoryService.GetAllLowStockAsync(branchThreshold);
        var groupedByBranch = lowStockBranchProducts.GroupBy(p => new { p.BranchId, p.BranchName });

        foreach (var group in groupedByBranch)
        {
            foreach (var product in group)
            {
                var message = $"Sản phẩm {product.ProductName} sắp hết hàng tại chi nhánh {group.Key.BranchName} (Còn lại: {product.Quantity}).";
                _logger.LogWarning(message);

                await notificationService.CreateAlertForBranchAsync(
                    group.Key.BranchId,
                    $"Cảnh báo Tồn kho - {group.Key.BranchName}",
                    message
                );
            }
        }
    }
}
