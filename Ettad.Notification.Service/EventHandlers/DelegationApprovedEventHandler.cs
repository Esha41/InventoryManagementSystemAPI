using Ettad.Notification.Service.Interfaces;
using Ettad.User.Services.Events;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Ettad.Notification.Service.EventHandlers
{
    public class DelegationApprovedEventHandler : INotificationHandler<DelegationApprovedEvent>
    {
        private readonly INotificationHelperService _notificationService;
        private readonly ILogger<DelegationApprovedEventHandler> _logger;

        public DelegationApprovedEventHandler(
            INotificationHelperService notificationService,
            ILogger<DelegationApprovedEventHandler> logger)
        {
            _notificationService = notificationService;
            _logger = logger;
        }

        public async Task Handle(DelegationApprovedEvent notification, CancellationToken cancellationToken)
        {
            try
            {
                var title = "Delegation Approved";
                var message = $"{notification.DelegateeName} has approved your delegation request.";

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

                _logger.LogInformation("Sent delegation approval notification for delegation {DelegationId}", notification.DelegationId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error sending delegation approval notification for delegation {DelegationId}", notification.DelegationId);
            }
        }
    }
}
