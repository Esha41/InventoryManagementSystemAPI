using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Ettad.Comman.Idenitity;
using Ettad.CrossCutting.Comman.Idenitity;
using Ettad.Notification.Service.Dtos;
using Ettad.Notification.Service.Hubs;

namespace Ettad.Notification.Service
{
    public class NotificationHelperService : INotificationHelperService
    {
        private readonly INotificationService _notificationService;
        private readonly IHubContext<NotificationHub> _hubContext;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<ApplicationRole> _roleManager;

        public NotificationHelperService(
            INotificationService notificationService,
            IHubContext<NotificationHub> hubContext,
            UserManager<ApplicationUser> userManager,
            RoleManager<ApplicationRole> roleManager)
        {
            _notificationService = notificationService;
            _hubContext = hubContext;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async Task SendNotificationAsync(
            string title, 
            string message, 
            string? entityType = null, 
            long? entityId = null, 
            List<string>? userIds = null, 
            List<string>? roleIds = null,
            string? senderId = null,
            bool includeSuperAdmins = false)
        {
            // Create notification DTO
            var createDto = new CreateNotificationDto
            {
                Title = title,
                Message = message,
                EntityType = entityType,
                EntityId = entityId,
                UserIds = userIds,
                RoleIds = roleIds,
                SenderId = senderId,
                IncludeSuperAdmins = includeSuperAdmins
            };

            // Save notification to database
            var result = await _notificationService.CreateNotificationAsync(createDto);
            if (!result.Succeeded)
            {
                // Log error but don't throw - notification creation failure shouldn't break the main flow
                return;
            }

            // Get all user IDs to notify (expand roles to users for SignalR delivery)
            var userIdsToNotify = new HashSet<string>();
            if (userIds != null && userIds.Any())
            {
                foreach (var userId in userIds)
                {
                    userIdsToNotify.Add(userId);
                }
            }

            if (roleIds != null && roleIds.Any())
            {
                foreach (var roleId in roleIds)
                {
                    var role = await _roleManager.FindByIdAsync(roleId);
                    if (role != null && !string.IsNullOrEmpty(role.Name))
                    {
                        var usersInRole = await _userManager.GetUsersInRoleAsync(role.Name);
                        foreach (var user in usersInRole)
                        {
                            userIdsToNotify.Add(user.Id);
                        }
                    }
                }
            }

            if (includeSuperAdmins)
            {
                var superAdminIds = await _userManager.Users
                    .Where(u => u.IsSuperAdmin)
                    .Select(u => u.Id)
                    .ToListAsync();

                foreach (var superAdminId in superAdminIds)
                {
                    userIdsToNotify.Add(superAdminId);
                }
            }

            // Send real-time notification via SignalR to all user groups
            var notificationDto = new Dtos.NotificationDto
            {
                Id = result.Data,
                Title = title,
                Message = message,
                EntityType = entityType,
                EntityId = entityId,
                SenderId = senderId,
                CreationDate = DateTime.UtcNow,
                IsRead = false
            };

            foreach (var userId in userIdsToNotify)
            {
                await _hubContext.Clients.Group($"user_{userId}")
                    .SendAsync("NotificationReceived", notificationDto);

                var unreadResult = await _notificationService.GetUnreadCountAsync(userId);
                if (unreadResult.Succeeded)
                {
                    await _hubContext.Clients.Group($"user_{userId}")
                        .SendAsync("UnreadCountUpdated", unreadResult.Data);
                }
            }
        }
    }
}
