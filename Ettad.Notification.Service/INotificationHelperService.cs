namespace Ettad.Notification.Service
{
    public interface INotificationHelperService
    {
        /// <summary>
        /// Sends a notification to specified users or roles.
        /// For entity-specific notifications, provide both entityType and entityId.
        /// For system/broadcast notifications, leave both null.
        /// </summary>
        Task SendNotificationAsync(
            string title, 
            string message, 
            string? entityType = null, 
            long? entityId = null, 
            List<string>? userIds = null, 
            List<string>? roleIds = null);
    }
}

