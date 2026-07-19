namespace ConvenienceStore.Application.DTOs.Notification;

public class NotificationListDto
{
    public IEnumerable<NotificationDto> Items { get; set; } = new List<NotificationDto>();
    public int Page { get; set; }
    public int PageSize { get; set; }
    public int TotalCount { get; set; }
}
