using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ettad.User.Services.Helpers
{
    public interface IEmailSender
    {
        /// <param name="isHtml">When false, body is sent as plain text.</param>
        Task SendEmailAsync(string email, string subject, string message, bool isHtml = true);
        Task SendEmailWithAttachmentAsync(string email, string subject, string message, byte[] attachmentBytes, string attachmentFileName, string? contentType = null);
    }
}
