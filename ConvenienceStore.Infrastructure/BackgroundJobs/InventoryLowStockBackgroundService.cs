using ConvenienceStore.Application.Contracts.External;
using ConvenienceStore.Application.Services.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace ConvenienceStore.Infrastructure.BackgroundJobs;

public class InventoryLowStockBackgroundService : BackgroundService
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<InventoryLowStockBackgroundService> _logger;
    private readonly TimeSpan _checkInterval = TimeSpan.FromMinutes(60);

    public InventoryLowStockBackgroundService(IServiceProvider serviceProvider, ILogger<InventoryLowStockBackgroundService> logger)
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
        var notificationService = scope.ServiceProvider.GetRequiredService<INotificationService>();

        _logger.LogInformation("Checking warehouse low stock...");
        
        var lowStockProducts = await warehouseService.GetLowStockProductsAsync(10);
        foreach (var product in lowStockProducts)
        {
            var message = $"Sản phẩm {product.ProductName} sắp hết hàng trong Kho Tổng (Còn lại: {product.Quantity}). Vui lòng kiểm tra và nhập thêm.";
            _logger.LogWarning(message);
            
            await notificationService.CreateAlertForRolesAsync(
                new List<string> { "Chain Director", "Warehouse Staff", "Store Manager" },
                "Cảnh báo Tồn kho Kho Tổng",
                message
            );
        }
    }
}
