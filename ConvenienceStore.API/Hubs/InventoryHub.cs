using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace ConvenienceStore.API.Hubs;

[Authorize]
public class InventoryHub : Hub
{
    public override async Task OnConnectedAsync()
    {
        // Join a group based on role
        if (Context.User?.IsInRole("Warehouse Staff") == true)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, "role-warehouse-staff");
        }
        else if (Context.User?.IsInRole("Store Manager") == true || Context.User?.IsInRole("Store Staff") == true)
        {
            // You can also add them to specific branch groups if you parse the branchId from claims
            var branchIdClaim = Context.User.FindFirst("BranchId")?.Value;
            if (!string.IsNullOrEmpty(branchIdClaim))
            {
                await Groups.AddToGroupAsync(Context.ConnectionId, $"branch-{branchIdClaim}");
            }
            await Groups.AddToGroupAsync(Context.ConnectionId, "role-store-manager");
        }

        await base.OnConnectedAsync();
    }

    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        await base.OnDisconnectedAsync(exception);
    }
}
