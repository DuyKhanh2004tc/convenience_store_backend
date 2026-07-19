using ConvenienceStore.API.Hubs;
using ConvenienceStore.Application.Contracts.External;
using Microsoft.AspNetCore.SignalR;

namespace ConvenienceStore.API.Services;

public class SignalRNotificationBroadcaster : INotificationBroadcaster
{
    private readonly IHubContext<InventoryHub> _hubContext;

    public SignalRNotificationBroadcaster(IHubContext<InventoryHub> hubContext)
    {
        _hubContext = hubContext;
    }

    public async Task BroadcastToUserAsync(int userId, object notification)
    {
        await _hubContext.Clients.Group($"user-{userId}").SendAsync("ReceiveNotification", notification);
    }
}
