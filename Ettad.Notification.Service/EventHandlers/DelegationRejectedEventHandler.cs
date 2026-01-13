using Ettad.Notification.Service;
using Ettad.User.Services.Events;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Ettad.Notification.Service.EventHandlers
{
    public class DelegationRejectedEventHandler : INotificationHandler<DelegationRejectedEvent>
    {
        private readonly INotificationHelperService _notificationService;
        private readonly ILogger<DelegationRejectedEventHandler> _logger;

        public DelegationRejectedEventHandler(
            INotificationHelperService notificationService,
            ILogger<DelegationRejectedEventHandler> logger)
        {
            _notificationService = notificationService;
            _logger = logger;
        }

        public async Task Handle(DelegationRejectedEvent notification, CancellationToken cancellationToken)
        {
            try
            {
                var title = "Delegation Rejected";
                var message = $"{notification.DelegateeName} has rejected your delegation request.";

                await _notificationService.SendNotificationAndEmailAsync(
                    title,
                    message,
                    "Delegation",
                    notification.DelegationId,
                    new List<string> { notification.DelegatorUserId },
                    null,
                    notification.DelegateeUserId,
                    false
                );

                _logger.LogInformation("Sent delegation rejection notification for delegation {DelegationId}", notification.DelegationId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error sending delegation rejection notification for delegation {DelegationId}", notification.DelegationId);
            }
        }
    }
}
