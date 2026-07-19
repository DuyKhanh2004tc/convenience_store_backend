using ConvenienceStore.Application.DTOs.Notification;

namespace ConvenienceStore.Application.Services.Interfaces;

public interface INotificationService
{
    Task CreateAuditLogAsync(int userId, string title, string message);
    Task CreateAlertForRolesAsync(List<string> roleNames, string title, string message);
    Task<NotificationListDto> GetNotificationsAsync(int userId, int page, int pageSize);
    Task<int> GetUnreadCountAsync(int userId);
    Task MarkAsReadAsync(int notificationId, int userId);
    Task MarkAllAsReadAsync(int userId);
}
