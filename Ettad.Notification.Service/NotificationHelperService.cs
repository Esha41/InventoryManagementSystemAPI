using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;
using Ettad.Comman.Idenitity;
using Ettad.CrossCutting.Comman.Idenitity;
using Ettad.Notification.Service.Dtos;
using Ettad.Notification.Service.Hubs;
using Ettad.User.Services.Helpers;
using Ettad.CrossCutting.Data.Repository;
using Ettad.Data.Entities;
using Ettad.Data.Enums;

namespace Ettad.Notification.Service
{
    public class NotificationHelperService : INotificationHelperService
    {
        private readonly INotificationService _notificationService;
        private readonly IHubContext<NotificationHub> _hubContext;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<ApplicationRole> _roleManager;
        private readonly IEmailSender _emailSender;
        private readonly ILogger<NotificationHelperService> _logger;
        private readonly IServiceProvider _serviceProvider;

        public NotificationHelperService(
            INotificationService notificationService,
            IHubContext<NotificationHub> hubContext,
            UserManager<ApplicationUser> userManager,
            RoleManager<ApplicationRole> roleManager,
            IEmailSender emailSender,
            ILogger<NotificationHelperService> logger,
            IServiceProvider serviceProvider)
        {
            _notificationService = notificationService;
            _hubContext = hubContext;
            _userManager = userManager;
            _roleManager = roleManager;
            _emailSender = emailSender;
            _logger = logger;
            _serviceProvider = serviceProvider;
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
                CreationDate = DateTime.UtcNow,
                IsRead = false
            };

            // Build email body with notification details (once for all users)
            var emailBody = await BuildEmailBodyAsync(title, message, entityType, entityId, result.Data);

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

                // Send email notification to this specific logged-in user
                await SendEmailToUserAsync(userId, title, emailBody);
            }
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
                // Log error but don't throw - email failure shouldn't break notification flow
                _logger.LogError(ex, "Failed to send email notification to user {UserId} for notification: {Title}. Error: {ErrorMessage}", 
                    userId, subject, ex.Message);
            }
        }

        private async Task<string> BuildEmailBodyAsync(string title, string message, string? entityType, long? entityId, long notificationId)
        {
            var now = DateTime.UtcNow;
            var emailBody = new System.Text.StringBuilder();
            emailBody.AppendLine("<html><body style='font-family: Arial, sans-serif; line-height: 1.6; color: #333; margin: 0; padding: 0; background-color: #f5f5f5;'>");
            emailBody.AppendLine("<div style='max-width: 800px; margin: 0 auto; padding: 20px; background-color: #ffffff;'>");
            
            // Header with title
            emailBody.AppendLine("<div style='margin-bottom: 20px; padding-bottom: 15px; border-bottom: 2px solid #e0e0e0;'>");
            emailBody.AppendLine($"<h1 style='margin: 0; color: #2c3e50; font-size: 24px; font-weight: 600;'>{title}</h1>");
            emailBody.AppendLine("</div>");
            
            // Introductory message
            emailBody.AppendLine($"<p style='font-size: 16px; color: #555; margin-bottom: 30px;'>{message}</p>");
            
            // Summary Cards Section
            emailBody.AppendLine("<div style='display: grid; grid-template-columns: repeat(auto-fit, minmax(180px, 1fr)); gap: 15px; margin-bottom: 30px;'>");
            
            // UPDATE DATE Card
            emailBody.AppendLine("<div style='background-color: #f8f9fa; border: 1px solid #e0e0e0; border-radius: 8px; padding: 15px;'>");
            emailBody.AppendLine("<div style='color: #7f8c8d; font-size: 12px; font-weight: 600; text-transform: uppercase; margin-bottom: 8px;'>UPDATE DATE</div>");
            emailBody.AppendLine($"<div style='color: #2c3e50; font-size: 14px; font-weight: 500;'>{now:MMM dd, yyyy}</div>");
            emailBody.AppendLine("</div>");
            
            // TIME Card
            emailBody.AppendLine("<div style='background-color: #f8f9fa; border: 1px solid #e0e0e0; border-radius: 8px; padding: 15px;'>");
            emailBody.AppendLine("<div style='color: #7f8c8d; font-size: 12px; font-weight: 600; text-transform: uppercase; margin-bottom: 8px;'>TIME</div>");
            emailBody.AppendLine($"<div style='color: #2c3e50; font-size: 14px; font-weight: 500;'>{now:h:mm tt}</div>");
            emailBody.AppendLine("</div>");
            
            emailBody.AppendLine("</div>"); // End summary cards grid

            // Fetch and include full entity details if available
            if (!string.IsNullOrEmpty(entityType) && entityId.HasValue)
            {
                var entityDetails = await GetEntityDetailsAsync(entityType, entityId.Value);
                if (entityDetails != null)
                {
                    emailBody.AppendLine($"<div style='margin-top: 30px;'>");
                    emailBody.AppendLine($"<h2 style='color: #2c3e50; font-size: 18px; font-weight: 600; margin-bottom: 20px; padding-bottom: 10px; border-bottom: 1px solid #e0e0e0;'>{entityType} details</h2>");
                    emailBody.AppendLine(entityDetails);
                    emailBody.AppendLine("</div>");
                }
                else
                {
                    // Fallback if entity details couldn't be fetched
                    emailBody.AppendLine("<div style='margin-top: 20px; padding: 15px; background-color: #e8f4f8; border-left: 4px solid #3498db; border-radius: 4px;'>");
                    emailBody.AppendLine($"<strong>Related Entity:</strong> {entityType}<br>");
                    emailBody.AppendLine($"<strong>Entity ID:</strong> {entityId.Value}");
                    emailBody.AppendLine("</div>");
                }
            }

            emailBody.AppendLine("<hr style='border: none; border-top: 1px solid #e0e0e0; margin: 30px 0 20px 0;'>");
            emailBody.AppendLine("<p style='color: #7f8c8d; font-size: 12px; text-align: center; margin: 0;'>This is an automated notification email.</p>");
            emailBody.AppendLine("</div></body></html>");

            return emailBody.ToString();
        }

        private async Task<string?> GetEntityDetailsAsync(string entityType, long entityId)
        {
            try
            {
                // Handle "Request" entityType (used by workflow notifications)
                if (string.Equals(entityType, "Request", StringComparison.OrdinalIgnoreCase))
                {
                    var baseRequestRepository = _serviceProvider.GetService<ICrossCuttingRepository<BaseRequest>>();
                    if (baseRequestRepository != null)
                    {
                        var request = await baseRequestRepository.FindOneAsync(
                            r => r.Id == entityId && !r.IsDeleted,
                            false,
                            nameof(BaseRequest.Department),
                            nameof(BaseRequest.Requester),
                            nameof(BaseRequest.RequestPurpose),
                            $"{nameof(BaseRequest.RequestItems)}.{nameof(RequestItem.Item)}"
                        );

                        if (request != null)
                        {
                            return FormatRequestDetails(request);
                        }
                    }
                }
                else if (string.Equals(entityType, "Discard", StringComparison.OrdinalIgnoreCase))
                {
                    var discardRepository = _serviceProvider.GetService<ICrossCuttingRepository<Discard>>();
                    if (discardRepository != null)
                    {
                        var discard = await discardRepository.FindOneAsync(
                            d => d.Id == entityId && !d.IsDeleted,
                            false,
                            nameof(BaseRequest.Department),
                            nameof(BaseRequest.Requester),
                            nameof(BaseRequest.RequestPurpose),
                            $"{nameof(BaseRequest.RequestItems)}.{nameof(RequestItem.Item)}"
                        );

                        if (discard != null)
                        {
                            return FormatRequestDetails(discard);
                        }
                    }
                }
                else if (string.Equals(entityType, "Return", StringComparison.OrdinalIgnoreCase))
                {
                    var returnRepository = _serviceProvider.GetService<ICrossCuttingRepository<Return>>();
                    if (returnRepository != null)
                    {
                        var returnEntity = await returnRepository.FindOneAsync(
                            r => r.Id == entityId && !r.IsDeleted,
                            false,
                            nameof(BaseRequest.Department),
                            nameof(BaseRequest.Requester),
                            nameof(BaseRequest.RequestPurpose),
                            $"{nameof(BaseRequest.RequestItems)}.{nameof(RequestItem.Item)}"
                        );

                        if (returnEntity != null)
                        {
                            return FormatRequestDetails(returnEntity);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Failed to fetch entity details for {EntityType} with ID {EntityId}", entityType, entityId);
            }

            return null;
        }

        private string FormatRequestDetails(BaseRequest request)
        {
            var details = new System.Text.StringBuilder();
            details.AppendLine("<div style='background-color: #ffffff;'>");
            
            // Request ID
            details.AppendLine("<div style='margin-bottom: 15px;'>");
            details.AppendLine("<div style='color: #7f8c8d; font-size: 12px; font-weight: 600; margin-bottom: 5px;'>REQUEST ID</div>");
            details.AppendLine($"<div style='color: #2c3e50; font-size: 14px;'>{request.RequestNo ?? "N/A"}</div>");
            details.AppendLine("</div>");
            
            // Department
            details.AppendLine("<div style='margin-bottom: 15px;'>");
            details.AppendLine("<div style='color: #7f8c8d; font-size: 12px; font-weight: 600; margin-bottom: 5px;'>DEPARTMENT</div>");
            details.AppendLine($"<div style='color: #2c3e50; font-size: 14px;'>{GetDepartmentName(request.Department)}</div>");
            details.AppendLine("</div>");
            
            // Requester
            details.AppendLine("<div style='margin-bottom: 15px;'>");
            details.AppendLine("<div style='color: #7f8c8d; font-size: 12px; font-weight: 600; margin-bottom: 5px;'>REQUESTER</div>");
            details.AppendLine($"<div style='color: #2c3e50; font-size: 14px;'>{GetRequesterName(request)}</div>");
            details.AppendLine("</div>");
            
            // Priority
            details.AppendLine("<div style='margin-bottom: 15px;'>");
            details.AppendLine("<div style='color: #7f8c8d; font-size: 12px; font-weight: 600; margin-bottom: 5px;'>PRIORITY</div>");
            details.AppendLine($"<div style='color: #2c3e50; font-size: 14px;'>{request.Priority}</div>");
            details.AppendLine("</div>");
            
            // Status
            details.AppendLine("<div style='margin-bottom: 15px;'>");
            details.AppendLine("<div style='color: #7f8c8d; font-size: 12px; font-weight: 600; margin-bottom: 5px;'>STATUS</div>");
            details.AppendLine($"<div style='color: #2c3e50; font-size: 14px;'>{request.Status}</div>");
            details.AppendLine("</div>");
            
            // Purpose
            details.AppendLine("<div style='margin-bottom: 15px;'>");
            details.AppendLine("<div style='color: #7f8c8d; font-size: 12px; font-weight: 600; margin-bottom: 5px;'>PURPOSE</div>");
            details.AppendLine($"<div style='color: #2c3e50; font-size: 14px;'>{GetRequestPurposeName(request.RequestPurpose)}</div>");
            details.AppendLine("</div>");
            
            // Reason (if provided)
            if (!string.IsNullOrEmpty(request.Reason))
            {
                details.AppendLine("<div style='margin-bottom: 15px;'>");
                details.AppendLine("<div style='color: #7f8c8d; font-size: 12px; font-weight: 600; margin-bottom: 5px;'>REASON</div>");
                details.AppendLine($"<div style='color: #2c3e50; font-size: 14px;'>{request.Reason}</div>");
                details.AppendLine("</div>");
            }
            
            // Notes (if provided)
            if (!string.IsNullOrEmpty(request.Notes))
            {
                details.AppendLine("<div style='margin-bottom: 15px;'>");
                details.AppendLine("<div style='color: #7f8c8d; font-size: 12px; font-weight: 600; margin-bottom: 5px;'>NOTES</div>");
                details.AppendLine($"<div style='color: #2c3e50; font-size: 14px;'>{request.Notes}</div>");
                details.AppendLine("</div>");
            }

            // Items
            if (request.RequestItems != null && request.RequestItems.Any())
            {
                details.AppendLine("<div style='margin-top: 20px; padding-top: 20px; border-top: 1px solid #e0e0e0;'>");
                details.AppendLine("<div style='color: #7f8c8d; font-size: 12px; font-weight: 600; margin-bottom: 15px;'>ITEMS</div>");
                foreach (var item in request.RequestItems)
                {
                    var itemName = item.Item?.Name ?? "N/A";
                    var itemNo = item.Item?.ItemNo ?? "N/A";
                    details.AppendLine("<div style='margin-bottom: 10px; padding: 10px; background-color: #f8f9fa; border-radius: 4px;'>");
                    details.AppendLine($"<div style='color: #2c3e50; font-size: 14px; font-weight: 500; margin-bottom: 5px;'>{itemName}</div>");
                    details.AppendLine($"<div style='color: #7f8c8d; font-size: 12px;'>Quantity: {item.Quantity}</div>");
                    details.AppendLine("</div>");
                }
                details.AppendLine("</div>");
            }

            details.AppendLine("</div>");
            return details.ToString();
        }

        private string GetDepartmentName(Department? department)
        {
            if (department != null)
            {
                if (!string.IsNullOrEmpty(department.NameEn))
                    return department.NameEn;
                if (!string.IsNullOrEmpty(department.NameAr))
                    return department.NameAr;
            }
            return "N/A";
        }

        private string GetRequestPurposeName(RequestPurpose? requestPurpose)
        {
            if (requestPurpose != null)
            {
                if (!string.IsNullOrEmpty(requestPurpose.NameEn))
                    return requestPurpose.NameEn;
                if (!string.IsNullOrEmpty(requestPurpose.NameAr))
                    return requestPurpose.NameAr;
            }
            return "N/A";
        }

        private string GetRequesterName(BaseRequest request)
        {
            if (request.Requester != null)
            {
                if (!string.IsNullOrEmpty(request.Requester.FullNameEN))
                    return request.Requester.FullNameEN;
                if (!string.IsNullOrEmpty(request.Requester.FullNameAR))
                    return request.Requester.FullNameAR;
                if (!string.IsNullOrEmpty(request.Requester.UserName))
                    return request.Requester.UserName;
            }
            return "N/A";
        }

        private void AddDetailRow(System.Text.StringBuilder sb, string label, string value)
        {
            sb.AppendLine("<tr>");
            sb.AppendLine($"<td style='padding: 8px 10px; font-weight: bold; width: 150px; border-bottom: 1px solid #f0f0f0;'>{label}:</td>");
            sb.AppendLine($"<td style='padding: 8px 10px; border-bottom: 1px solid #f0f0f0;'>{value}</td>");
            sb.AppendLine("</tr>");
        }
    }
}
