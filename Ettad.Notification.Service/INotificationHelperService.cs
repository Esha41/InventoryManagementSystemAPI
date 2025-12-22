namespace Ettad.Notification.Service
{
    public interface INotificationHelperService
    {
        /// <summary>
        /// Sends a notification (SignalR + Database) to specified users or roles.
        /// For entity-specific notifications, provide both entityType and entityId.
        /// For system/broadcast notifications, leave both null.
        /// </summary>
        Task SendNotificationAsync(
            string title, 
            string message, 
            string? entityType = null, 
            long? entityId = null, 
            List<string>? userIds = null, 
            List<string>? roleIds = null,
            string? senderId = null,
            bool includeSuperAdmins = false);

        /// <summary>
        /// Sends email notifications to specified users or roles.
        /// </summary>
        Task SendEmailAsync(
            string title,
            string message,
            string? entityType = null,
            long? entityId = null,
            List<string>? userIds = null,
            List<string>? roleIds = null,
            bool includeSuperAdmins = false,
            string? htmlContent = null);

        /// <summary>
        /// Sends both notification (SignalR + Database) and email to specified users or roles.
        /// </summary>
        Task SendNotificationAndEmailAsync(
            string title,
            string message,
            string? entityType = null,
            long? entityId = null,
            List<string>? userIds = null,
            List<string>? roleIds = null,
            string? senderId = null,
            bool includeSuperAdmins = false);
    }
}

