namespace Ettad.Notification.Service.EmailTemplate
{
    public interface IEmailTemplateService
    {
        /// <summary>
        /// Renders the email HTML template with the provided parameters
        /// </summary>
        Task<string> RenderEmailTemplateAsync(
            string title,
            string message,
            string? entityType = null,
            long? entityId = null,
            long notificationId = 0,
            DateTime? updateDate = null);
    }
}

