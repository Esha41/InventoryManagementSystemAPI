namespace Ettad.Notification.Service.Interfaces
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
            bool includeSuperAdmins = false,
            bool includeAllUsers = false);

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
            bool includeAllUsers = false,
            string? htmlContent = null);

        /// <summary>
        /// Broadcasts a lightweight "workflow state changed" signal (SignalR only, no DB row, no email)
        /// to every client currently subscribed to this request's group. Clients react by re-fetching
        /// the request so their open page reflects the latest state.
        /// </summary>
        Task SendWorkflowStateChangedAsync(long requestId);

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
            bool includeSuperAdmins = false,
            bool includeAllUsers = false);
    }
}

