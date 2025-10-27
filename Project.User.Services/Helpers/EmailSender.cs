using Microsoft.Extensions.Logging;
using Ettad.Services.Helpers;
using Ettad.User.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace Ettad.User.Services.Helpers
{
    public class EmailSender : IEmailSender
    {
        private readonly ILogger<EmailSender> _logger;
        private readonly IHelpureService _helpureService;

        public EmailSender( ILogger<EmailSender> logger , IHelpureService helpureService)
        {
            _logger = logger;
            _helpureService = helpureService;
        }

        public async Task SendEmailAsync(string email, string subject, string message)
        {
            try
            {
                var emailConfig = await _helpureService.GetEmailConfigrationAsync(1);
                if (emailConfig == null)
                    throw new InvalidOperationException("Email configuration not found.");

                if (string.IsNullOrEmpty(emailConfig.HostIp) || emailConfig.Port == 0 ||
                    string.IsNullOrEmpty(emailConfig.Username) || string.IsNullOrEmpty(emailConfig.Password))
                {
                    throw new InvalidOperationException("Invalid email configuration settings.");
                }

                using var client = new SmtpClient(emailConfig.HostIp)
                {
                    Port = emailConfig.Port,
                    Credentials = new NetworkCredential(emailConfig.Username, emailConfig.Password),
                    EnableSsl = true,
                    UseDefaultCredentials = false,
                    Timeout = 10000 // 10 ?????
                };

                var mailMessage = new MailMessage
                {
                    From = new MailAddress(emailConfig.Username),
                    Subject = subject,
                    Body = message,
                    IsBodyHtml = true
                };

                mailMessage.To.Add(email);

                await client.SendMailAsync(mailMessage);
                _logger.LogInformation($"Email sent successfully to {email}");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Failed to send email to {email}: {ex.Message}");
                throw new Exception($"Email sending failed: {ex.Message}");
            }
        }
    }
}
