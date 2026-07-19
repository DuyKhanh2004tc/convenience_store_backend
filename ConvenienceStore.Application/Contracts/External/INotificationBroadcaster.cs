namespace ConvenienceStore.Application.Contracts.External;

public interface INotificationBroadcaster
{
    Task BroadcastToUserAsync(int userId, object notification);
}
