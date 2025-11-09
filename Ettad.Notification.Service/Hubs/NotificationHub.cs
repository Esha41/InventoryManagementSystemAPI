using Microsoft.AspNetCore.SignalR;

namespace Ettad.Notification.Service.Hubs
{
    public class NotificationHub : Hub
    {
        public async Task JoinUserGroup(string userId)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, $"user_{userId}");
        }

        public async Task LeaveUserGroup(string userId)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, $"user_{userId}");
        }

        public async Task JoinRoleGroup(string roleId)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, $"role_{roleId}");
        }

        public async Task LeaveRoleGroup(string roleId)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, $"role_{roleId}");
        }
    }
}

