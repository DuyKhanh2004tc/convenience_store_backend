using ConvenienceStore.Application.Contracts.Persistence;
using ConvenienceStore.Application.DTOs.Notification;
using ConvenienceStore.Application.Services.Interfaces;
using ConvenienceStore.Domain.Entities;

namespace ConvenienceStore.Application.Services;

public class NotificationService : INotificationService
{
    private readonly IGenericRepository<Notification> _notificationRepository;
    private readonly IUnitOfWork _unitOfWork;

    public NotificationService(
        IGenericRepository<Notification> notificationRepository,
        IUnitOfWork unitOfWork)
    {
        _notificationRepository = notificationRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task CreateAuditLogAsync(int userId, string title, string message)
    {
        if (title.Length > 255)
        {
            title = title[..255];
        }

        var notification = new Notification
        {
            UserId = userId,
            Title = title,
            Message = message,
            IsRead = false,
            CreatedAt = DateTime.UtcNow
        };

        await _notificationRepository.AddAsync(notification);
        await _unitOfWork.SaveChangesAsync();
    }

    public async Task<NotificationListDto> GetNotificationsAsync(int userId, int page, int pageSize)
    {
        var query = await _notificationRepository.GetAsync(n => n.UserId == userId);
        
        var totalCount = query.Count();
        var items = query
            .OrderByDescending(n => n.CreatedAt)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(n => new NotificationDto
            {
                NotificationId = n.NotificationId,
                UserId = n.UserId,
                Title = n.Title,
                Message = n.Message,
                IsRead = n.IsRead,
                CreatedAt = n.CreatedAt
            }).ToList();

        return new NotificationListDto
        {
            Items = items,
            Page = page,
            PageSize = pageSize,
            TotalCount = totalCount
        };
    }

    public async Task<int> GetUnreadCountAsync(int userId)
    {
        var query = await _notificationRepository.GetAsync(n => n.UserId == userId && !n.IsRead);
        return query.Count();
    }

    public async Task MarkAsReadAsync(int notificationId, int userId)
    {
        var notification = await _notificationRepository.GetByIdAsync(notificationId);
        if (notification != null && notification.UserId == userId && !notification.IsRead)
        {
            notification.IsRead = true;
            _notificationRepository.Update(notification);
            await _unitOfWork.SaveChangesAsync();
        }
    }

    public async Task MarkAllAsReadAsync(int userId)
    {
        var notifications = await _notificationRepository.GetAsync(n => n.UserId == userId && !n.IsRead);
        var unreadList = notifications.ToList();
        
        if (unreadList.Any())
        {
            foreach (var n in unreadList)
            {
                n.IsRead = true;
                _notificationRepository.Update(n);
            }
            await _unitOfWork.SaveChangesAsync();
        }
    }
}
