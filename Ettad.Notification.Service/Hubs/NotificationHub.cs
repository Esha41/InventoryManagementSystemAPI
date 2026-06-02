using Microsoft.AspNetCore.SignalR;
using System;

namespace Ettad.Notification.Service.Hubs
{
    public class NotificationHub : Hub
    {
        public override async Task OnConnectedAsync()
        {
            var userId = Context.GetHttpContext()?.Request.Query["userId"].ToString();
            if (!string.IsNullOrWhiteSpace(userId))
            {
                await Groups.AddToGroupAsync(Context.ConnectionId, $"user_{userId}");
            }

            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            var userId = Context.GetHttpContext()?.Request.Query["userId"].ToString();
            if (!string.IsNullOrWhiteSpace(userId))
            {
                await Groups.RemoveFromGroupAsync(Context.ConnectionId, $"user_{userId}");
            }

            await base.OnDisconnectedAsync(exception);
        }

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

        /// <summary>
        /// Subscribe this connection to live updates for a single request (e.g. while the user has the
        /// workflow approval detail page open). Mirrors the role/user group pattern.
        /// </summary>
        public async Task JoinRequestGroup(string requestId)
        {
            if (!string.IsNullOrWhiteSpace(requestId))
            {
                await Groups.AddToGroupAsync(Context.ConnectionId, $"request_{requestId}");
            }
        }

        public async Task LeaveRequestGroup(string requestId)
        {
            if (!string.IsNullOrWhiteSpace(requestId))
            {
                await Groups.RemoveFromGroupAsync(Context.ConnectionId, $"request_{requestId}");
            }
        }
    }
}

