using Ettad.Announcement.Service.Interfaces;
using Ettad.User.Services.Events;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Ettad.Announcement.Service.EventHandlers
{
    /// <summary>
    /// Clears announcement dismissals when a user logs out so banners reappear on next login.
    /// </summary>
    public class UserLoggedOutEventHandler : INotificationHandler<UserLoggedOutEvent>
    {
        private readonly IAnnouncementService _announcementService;
        private readonly ILogger<UserLoggedOutEventHandler> _logger;

        public UserLoggedOutEventHandler(
            IAnnouncementService announcementService,
            ILogger<UserLoggedOutEventHandler> logger)
        {
            _announcementService = announcementService;
            _logger = logger;
        }

        public async Task Handle(UserLoggedOutEvent notification, CancellationToken cancellationToken)
        {
            try
            {
                await _announcementService.ClearDismissalsForUserAsync(notification.UserId, cancellationToken);
                _logger.LogDebug("Cleared announcement dismissals for user on logout. UserId: {UserId}", notification.UserId);
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Failed to clear announcement dismissals on logout. UserId: {UserId}", notification.UserId);
            }
        }
    }
}
