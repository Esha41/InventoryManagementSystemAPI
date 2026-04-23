using Ettad.User.Services.Events;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Ettad.Comman.Idenitity;
using Ettad.Notification.Service.Interfaces;

namespace Ettad.Notification.Service.EventHandlers
{
    public class DelegationCreatedEventHandler : INotificationHandler<DelegationCreatedEvent>
    {
        private readonly INotificationHelperService _notificationService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ILogger<DelegationCreatedEventHandler> _logger;

        public DelegationCreatedEventHandler(
            INotificationHelperService notificationService,
            UserManager<ApplicationUser> userManager,
            ILogger<DelegationCreatedEventHandler> logger)
        {
            _notificationService = notificationService;
            _userManager = userManager;
            _logger = logger;
        }

        public async Task Handle(DelegationCreatedEvent notification, CancellationToken cancellationToken)
        {
            try
            {
                var delegator = await _userManager.FindByIdAsync(notification.DelegatorUserId);
                var delegatorName = delegator?.FullNameEN ?? delegator?.FullNameAR ?? delegator?.UserName ?? "Unknown";

                var title = "New Delegation Request";
                var message = $"{delegatorName} has requested to delegate their authority to you from {notification.StartDate:yyyy-MM-dd} to {notification.EndDate:yyyy-MM-dd}. Reason: {notification.Reason}. Please review and approve or reject this delegation.";

                await _notificationService.SendNotificationAndEmailAsync(
                    title,
                    message,
                    "Delegation",
                    notification.DelegationId,
                    new List<string> { notification.DelegateeUserId },
                    null,
                    notification.DelegatorUserId,
                    false
                );

                _logger.LogInformation("Sent delegation creation notification for delegation {DelegationId}", notification.DelegationId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error sending delegation creation notification for delegation {DelegationId}", notification.DelegationId);
            }
        }
    }
}
