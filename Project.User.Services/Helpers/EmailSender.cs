using Microsoft.Extensions.Logging;
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
        private readonly ISettingsProvider _settingsProvider;

        public EmailSender(ILogger<EmailSender> logger, ISettingsProvider settingsProvider)
        {
            _logger = logger;
            _settingsProvider = settingsProvider;
        }

        public async Task SendEmailAsync(string email, string subject, string message)
        {
            try
            {
                _logger.LogInformation("Attempting to send email to {Email} with subject: {Subject}", email, subject);
                
                var emailConfig = await _settingsProvider.getEmailSettings();
                
                // Check if email notifications are disabled
                if (emailConfig.DisableAuthentication)
                {
                    _logger.LogWarning("Email notifications are disabled in settings. Skipping email send.");
                    return;
                }

                // Check if email configuration is valid
                if (string.IsNullOrEmpty(emailConfig.HostIp) || emailConfig.Port == 0 ||
                    string.IsNullOrEmpty(emailConfig.Username) || string.IsNullOrEmpty(emailConfig.Password))
                {
                    _logger.LogError("Email configuration not found or invalid: HostIp={HostIp}, Port={Port}, Username={Username}, Password={HasPassword}", 
                        emailConfig.HostIp ?? "null", emailConfig.Port, emailConfig.Username ?? "null", !string.IsNullOrEmpty(emailConfig.Password));
                    throw new InvalidOperationException("Email configuration not found or invalid. Please configure email settings via /api/EmailSettings endpoint.");
                }

                _logger.LogInformation("Email configuration retrieved. Host: {Host}, Port: {Port}, Username: {Username}", 
                    emailConfig.HostIp, emailConfig.Port, emailConfig.Username);

                using var client = new SmtpClient(emailConfig.HostIp, emailConfig.Port)
                {
                    Credentials = new NetworkCredential(emailConfig.Username, emailConfig.Password),
                    EnableSsl = emailConfig.SSL,
                    UseDefaultCredentials = false,
                    Timeout = 10000
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
