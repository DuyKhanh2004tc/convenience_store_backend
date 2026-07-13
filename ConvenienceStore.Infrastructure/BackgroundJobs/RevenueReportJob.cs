using ConvenienceStore.Application.Contracts.Persistence;
using ConvenienceStore.Domain.Entities;
using ConvenienceStore.Domain.Enums;

namespace ConvenienceStore.Infrastructure.BackgroundJobs;

public interface IRevenueReportJob
{
    Task CalculateDailyRevenueAsync();
}

public class RevenueReportJob : IRevenueReportJob
{
    private readonly IGenericRepository<Invoice> _invoiceRepository;
    private readonly IGenericRepository<User> _userRepository;
    private readonly IGenericRepository<Role> _roleRepository;
    private readonly IGenericRepository<Notification> _notificationRepository;
    private readonly IUnitOfWork _unitOfWork;

    public RevenueReportJob(
        IGenericRepository<Invoice> invoiceRepository,
        IGenericRepository<User> userRepository,
        IGenericRepository<Role> roleRepository,
        IGenericRepository<Notification> notificationRepository,
        IUnitOfWork unitOfWork)
    {
        _invoiceRepository = invoiceRepository;
        _userRepository = userRepository;
        _roleRepository = roleRepository;
        _notificationRepository = notificationRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task CalculateDailyRevenueAsync()
    {
        // Calculate for the day that just ended. Since it runs at 23:59, we use today's date.
        var today = DateTime.UtcNow.Date;
        var endOfDay = today.AddDays(1);

        // Get all completed invoices for today
        var invoices = await _invoiceRepository.GetAsync(i => 
            i.InvoiceDate >= today && 
            i.InvoiceDate < endOfDay &&
            i.Status == OrderStatus.Completed);

        if (!invoices.Any())
            return; // Nothing to report

        var totalRevenue = invoices.Sum(i => i.FinalAmount);
        
        // Find Admin or Chain Director roles
        var adminRoles = await _roleRepository.GetAsync(r => r.RoleName == "System Admin" || r.RoleName == "Chain Director");
        var adminRoleIds = adminRoles.Select(r => r.RoleId).ToList();

        if (!adminRoleIds.Any())
            return;

        // Get all admin users
        var adminUsers = await _userRepository.GetAsync(u => adminRoleIds.Contains(u.RoleId) && u.IsActive);

        var notifications = new List<Notification>();
        var message = $"Báo cáo doanh thu cuối ngày ({today:dd/MM/yyyy}): Hệ thống đã ghi nhận {invoices.Count} hóa đơn, với tổng doanh thu {totalRevenue:N0} VND.";

        foreach (var user in adminUsers)
        {
            notifications.Add(new Notification
            {
                UserId = user.UserId,
                Title = "Báo cáo doanh thu ngày",
                Message = message,
                IsRead = false,
                CreatedAt = DateTime.UtcNow
            });
        }

        if (notifications.Any())
        {
            await _notificationRepository.AddRangeAsync(notifications);
            await _unitOfWork.SaveChangesAsync();
        }
    }
}
