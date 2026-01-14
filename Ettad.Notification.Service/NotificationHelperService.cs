using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;
using Ettad.Comman.Idenitity;
using Ettad.CrossCutting.Comman.Idenitity;
using Ettad.Notification.Service.Dtos;
using Ettad.Notification.Service.Hubs;
using Ettad.Notification.Service.EmailTemplate;
using Ettad.User.Services.Helpers;
using Ettad.CrossCutting.Comman.Time;

namespace Ettad.Notification.Service
{
    public class NotificationHelperService : INotificationHelperService
    {
        private readonly INotificationService _notificationService;
        private readonly IHubContext<NotificationHub> _hubContext;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<ApplicationRole> _roleManager;
        private readonly IEmailSender _emailSender;
        private readonly IEmailTemplateService _emailTemplateService;
        private readonly ILogger<NotificationHelperService> _logger;

        public NotificationHelperService(
            INotificationService notificationService,
            IHubContext<NotificationHub> hubContext,
            UserManager<ApplicationUser> userManager,
            RoleManager<ApplicationRole> roleManager,
            IEmailSender emailSender,
            IEmailTemplateService emailTemplateService,
            ILogger<NotificationHelperService> logger,
            IDateTimeProvider dateTimeProvider)
        {
            _notificationService = notificationService;
            _hubContext = hubContext;
            _userManager = userManager;
            _roleManager = roleManager;
            _emailSender = emailSender;
            _emailTemplateService = emailTemplateService;
            _logger = logger;
            _dateTimeProvider = dateTimeProvider;
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
                _logger.LogWarning("Failed to create notification in database. Title: {Title}, Message: {Message}, Error: {Error}", 
                    title, message, result.Message ?? "Unknown error");
                return;
            }

            _logger.LogInformation("Notification created successfully in database. NotificationId: {NotificationId}, Title: {Title}", 
                result.Data, title);

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
                CreationDate = _dateTimeProvider.Now,
                IsRead = false
            };

            _logger.LogInformation("Preparing to send notifications to {UserCount} users. Title: {Title}", 
                userIdsToNotify.Count, title);

            foreach (var userId in userIdsToNotify)
            {
                // Send real-time notification via SignalR
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

        public async Task SendEmailAsync(
            string title,
            string message,
            string? entityType = null,
            long? entityId = null,
            List<string>? userIds = null,
            List<string>? roleIds = null,
            bool includeSuperAdmins = false,
            string? htmlContent = null)
        {
            try
            {
                // Get all user IDs to send emails to
                var userIdsToEmail = await GetUserIdsAsync(userIds, roleIds, includeSuperAdmins);

                if (!userIdsToEmail.Any())
                {
                    _logger.LogWarning("No users found to send email to. Title: {Title}", title);
                    return;
                }

                _logger.LogInformation("Preparing to send emails to {UserCount} users. Title: {Title}, EntityType: {EntityType}, EntityId: {EntityId}", 
                    userIdsToEmail.Count, title, entityType ?? "None", entityId ?? 0);

                // Build email body once for all users
                var emailBody = await _emailTemplateService.RenderEmailTemplateAsync(
                    title, message, entityType, entityId, 0, _dateTimeProvider.Now, htmlContent);

                _logger.LogInformation("Email template rendered successfully. Body length: {BodyLength} characters", emailBody?.Length ?? 0);

                foreach (var userId in userIdsToEmail)
                {
                    await SendEmailToUserAsync(userId, title, emailBody);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in SendEmailAsync. Title: {Title}, EntityType: {EntityType}, EntityId: {EntityId}", 
                    title, entityType ?? "None", entityId ?? 0);
                throw; // Re-throw to allow caller to handle
            }
        }

        public async Task SendNotificationAndEmailAsync(
            string title,
            string message,
            string? entityType = null,
            long? entityId = null,
            List<string>? userIds = null,
            List<string>? roleIds = null,
            string? senderId = null,
            bool includeSuperAdmins = false)
        {
            // Send notification first
            await SendNotificationAsync(title, message, entityType, entityId, userIds, roleIds, senderId, includeSuperAdmins);
            
            // Then send email
            await SendEmailAsync(title, message, entityType, entityId, userIds, roleIds, includeSuperAdmins);
        }

        private async Task<HashSet<string>> GetUserIdsAsync(
            List<string>? userIds,
            List<string>? roleIds,
            bool includeSuperAdmins)
        {
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

            return userIdsToNotify;
        }

        private async Task SendEmailToUserAsync(string userId, string subject, string emailBody)
        {
            try
            {
                // Get the user's email address
                var user = await _userManager.FindByIdAsync(userId);
                if (user == null)
                {
                    _logger.LogWarning("User {UserId} not found, skipping email notification for: {Title}", userId, subject);
                    return;
                }

                if (string.IsNullOrEmpty(user.Email))
                {
                    _logger.LogWarning("User {UserId} ({UserName}) has no email address configured, skipping email notification for: {Title}", 
                        userId, user.UserName ?? "Unknown", subject);
                    return;
                }

                _logger.LogInformation("Attempting to send email notification to {Email} (UserId: {UserId}, UserName: {UserName}) for notification: {Title}", 
                    user.Email, userId, user.UserName ?? "Unknown", subject);

                // Send email to this specific user
                await _emailSender.SendEmailAsync(user.Email, subject, emailBody);
                _logger.LogInformation("Email notification sent successfully to {Email} (UserId: {UserId}) for notification: {Title}", 
                    user.Email, userId, subject);
            }
            catch (Exception ex)
            {
                // Log error with full details but don't throw - email failure shouldn't break notification flow
                _logger.LogError(ex, "Failed to send email notification to user {UserId} for notification: {Title}. Error: {ErrorMessage}. StackTrace: {StackTrace}", 
                    userId, subject, ex.Message, ex.StackTrace);
            }
        }

    }
}
