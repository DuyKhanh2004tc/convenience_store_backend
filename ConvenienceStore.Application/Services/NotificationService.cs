using ConvenienceStore.Application.Contracts.Persistence;
using ConvenienceStore.Application.DTOs.Notification;
using ConvenienceStore.Application.Services.Interfaces;
using ConvenienceStore.Application.Contracts.External;
using ConvenienceStore.Domain.Entities;

namespace ConvenienceStore.Application.Services;

public class NotificationService : INotificationService
{
    private readonly IGenericRepository<Notification> _notificationRepository;
    private readonly IGenericRepository<User> _userRepository;
    private readonly IGenericRepository<Role> _roleRepository;
    private readonly INotificationBroadcaster _broadcaster;
    private readonly IUnitOfWork _unitOfWork;

    public NotificationService(
        IGenericRepository<Notification> notificationRepository,
        IGenericRepository<User> userRepository,
        IGenericRepository<Role> roleRepository,
        INotificationBroadcaster broadcaster,
        IUnitOfWork unitOfWork)
    {
        _notificationRepository = notificationRepository;
        _userRepository = userRepository;
        _roleRepository = roleRepository;
        _broadcaster = broadcaster;
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

        var notificationDto = new NotificationDto
        {
            NotificationId = notification.NotificationId,
            UserId = notification.UserId,
            Title = notification.Title,
            Message = notification.Message,
            IsRead = notification.IsRead,
            CreatedAt = notification.CreatedAt
        };
        await _broadcaster.BroadcastToUserAsync(userId, notificationDto);
    }

    public async Task CreateAlertForRolesAsync(List<string> roleNames, string title, string message)
    {
        if (title.Length > 255)
        {
            title = title[..255];
        }

        var roles = await _roleRepository.GetAsync(r => roleNames.Contains(r.RoleName));
        var roleIds = roles.Select(r => r.RoleId).ToList();

        if (!roleIds.Any()) return;

        var users = await _userRepository.GetAsync(u => roleIds.Contains(u.RoleId) && u.IsActive);
        
        foreach (var user in users)
        {
            var notification = new Notification
            {
                UserId = user.UserId,
                Title = title,
                Message = message,
                IsRead = false,
                CreatedAt = DateTime.UtcNow
            };
            
            await _notificationRepository.AddAsync(notification);
        }
        
        await _unitOfWork.SaveChangesAsync();

        // Broadcast separately after all DB updates are committed
        foreach (var user in users)
        {
            // We fetch the latest notification Id. Since it was just saved, we can query it.
            // Or just broadcast the DTO without accurate ID if we don't query it.
            // For simplicity, we just send a reload signal or partial DTO.
            var dto = new NotificationDto
            {
                NotificationId = 0, // Frontend will usually re-fetch if they want full details, but let's just pass 0
                UserId = user.UserId,
                Title = title,
                Message = message,
                IsRead = false,
                CreatedAt = DateTime.UtcNow
            };
            await _broadcaster.BroadcastToUserAsync(user.UserId, dto);
        }
    }

    public async Task CreateAlertForBranchAsync(int branchId, string title, string message)
    {
        if (title.Length > 255)
        {
            title = title[..255];
        }

        // Send to Store Manager and Store Staff of the specific branch
        var targetRoles = new List<string> { "Store Manager", "Store Staff" };
        var roles = await _roleRepository.GetAsync(r => targetRoles.Contains(r.RoleName));
        var roleIds = roles.Select(r => r.RoleId).ToList();

        var users = await _userRepository.GetAsync(u => u.BranchId == branchId && roleIds.Contains(u.RoleId) && u.IsActive);
        
        foreach (var user in users)
        {
            var notification = new Notification
            {
                UserId = user.UserId,
                Title = title,
                Message = message,
                IsRead = false,
                CreatedAt = DateTime.UtcNow
            };
            
            await _notificationRepository.AddAsync(notification);
        }
        
        await _unitOfWork.SaveChangesAsync();

        foreach (var user in users)
        {
            var dto = new NotificationDto
            {
                NotificationId = 0,
                UserId = user.UserId,
                Title = title,
                Message = message,
                IsRead = false,
                CreatedAt = DateTime.UtcNow
            };
            await _broadcaster.BroadcastToUserAsync(user.UserId, dto);
        }
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
