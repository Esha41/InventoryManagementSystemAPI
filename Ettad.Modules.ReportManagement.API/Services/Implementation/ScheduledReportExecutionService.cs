using Ettad.CrossCutting.Comman.Time;
using Ettad.CrossCutting.Data.Repository;
using Ettad.Data.Entities;
using DevExpress.XtraReports.UI;
using Ettad.Modules.ReportManagement.API.Reports.Factories;
using Ettad.User.Services.Helpers;
using Ettad.User.Services.Interfaces;
using Ettad.Modules.ReportManagement.API.Services.Interfaces;

namespace Ettad.Modules.ReportManagement.API.Services.Implementation
{
    public class ScheduledReportExecutionService : IScheduledReportExecutionService
    {
        private readonly ICrossCuttingRepository<ScheduledReport> _scheduledReportRepository;
        private readonly ICrossCuttingRepository<ScheduledReportExecution> _executionRepository;
        private readonly IDateTimeProvider _dateTimeProvider;
        private readonly ILogger<ScheduledReportExecutionService> _logger;
        private readonly IReportService _reportService;
        private readonly ReportFactory _reportFactory;
        private readonly IEmailSender _emailSender;
        private readonly IUserService _userService;

        public ScheduledReportExecutionService(
            ICrossCuttingRepository<ScheduledReport> scheduledReportRepository,
            ICrossCuttingRepository<ScheduledReportExecution> executionRepository,
            IDateTimeProvider dateTimeProvider,
            ILogger<ScheduledReportExecutionService> logger,
            IReportService reportService,
            ReportFactory reportFactory,
            IEmailSender emailSender,
            IUserService userService)
        {
            _scheduledReportRepository = scheduledReportRepository;
            _executionRepository = executionRepository;
            _dateTimeProvider = dateTimeProvider;
            _logger = logger;
            _reportService = reportService;
            _reportFactory = reportFactory;
            _emailSender = emailSender;
            _userService = userService;
        }

        public async Task ExecuteScheduledReportsAsync()
        {
            _logger.LogInformation("Checking for scheduled reports to execute...");

            try
            {
                var now = _dateTimeProvider.Now;
                var scheduledReports = await _scheduledReportRepository.FindAsync(
                    sr => sr.IsActive 
                        && !sr.IsDeleted 
                        && sr.NextRunDate.HasValue 
                        && sr.NextRunDate.Value <= now,
                    false,
                    nameof(ScheduledReport.Report),
                    nameof(ScheduledReport.Recipients)
                );

             //   _logger.LogInformation("Found {Count} scheduled reports ready to execute", scheduledReports.Count);

                foreach (var scheduledReport in scheduledReports)
                {
                    try
                    {
                        await ExecuteReportAsync(scheduledReport.Id);
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "Error executing scheduled report {Id}", scheduledReport.Id);
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error checking for scheduled reports");
            }
        }

        public async Task ExecuteReportAsync(Guid scheduledReportId)
        {
            _logger.LogInformation("Executing scheduled report {Id}", scheduledReportId);

            var execution = new ScheduledReportExecution
            {
                Id = Guid.NewGuid(),
                ScheduledReportId = scheduledReportId,
                ExecutionDate = _dateTimeProvider.Now,
                Status = "Success",
                CreationDate = _dateTimeProvider.Now
            };

            try
            {
                var scheduledReport = await _scheduledReportRepository.FindOneAsync(
                    sr => sr.Id == scheduledReportId && !sr.IsDeleted,
                    false,
                    nameof(ScheduledReport.Report),
                    nameof(ScheduledReport.Recipients)
                );

                if (scheduledReport == null || scheduledReport.IsDeleted || !scheduledReport.IsActive)
                {
                    _logger.LogWarning("Scheduled report {Id} not found or inactive", scheduledReportId);
                    return;
                }

                if (scheduledReport.Report == null)
                {
                    _logger.LogWarning("Report not found for scheduled report {Id}", scheduledReportId);
                    execution.Status = "Failed";
                    execution.ErrorMessage = "Report not found";
                    await SaveExecutionAsync(execution);
                    return;
                }

                // Get recipients - fetch email from UserId if present, otherwise use EmailAddress (for manual entries only)
                var recipientEmails = new List<string>();
                foreach (var recipient in scheduledReport.Recipients.Where(r => !r.IsDeleted))
                {
                    string? email = null;
                    
                    // If UserId is present, get the current email from the user (EmailAddress is not stored for users)
                    if (!string.IsNullOrWhiteSpace(recipient.UserId))
                    {
                        try
                        {
                            var userResult = await _userService.GetByIdAsync(recipient.UserId);
                            if (userResult.Succeeded && userResult.Data != null && !string.IsNullOrWhiteSpace(userResult.Data.Email))
                            {
                                email = userResult.Data.Email;
                                _logger.LogInformation("Retrieved email {Email} for user {UserId}", email, recipient.UserId);
                            }
                            else
                            {
                                _logger.LogWarning("User {UserId} not found or has no email address", recipient.UserId);
                            }
                        }
                        catch (Exception ex)
                        {
                            _logger.LogError(ex, "Error retrieving email for user {UserId}", recipient.UserId);
                        }
                    }
                    else
                    {
                        // For manual email entries (no UserId), use stored EmailAddress
                        email = recipient.EmailAddress;
                    }
                    
                    if (!string.IsNullOrWhiteSpace(email))
                    {
                        recipientEmails.Add(email);
                    }
                }

                var recipients = recipientEmails.Distinct().ToList();

                if (!recipients.Any())
                {
                    _logger.LogWarning("No recipients found for scheduled report {Id}", scheduledReportId);
                    execution.Status = "Failed";
                    execution.ErrorMessage = "No recipients configured";
                    await SaveExecutionAsync(execution);
                    return;
                }

                // Generate report directly using DevExpress
                var reportBytes = await GenerateReportBytesAsync(
                    scheduledReport.Report.Url,
                    scheduledReport.OutputFormat
                );

                if (reportBytes == null || reportBytes.Length == 0)
                {
                    execution.Status = "Failed";
                    execution.ErrorMessage = "Failed to generate report";
                    await SaveExecutionAsync(execution);
                    return;
                }

                execution.FileSizeBytes = reportBytes.Length;
                execution.RecipientCount = recipients.Count;

                // Send email with report attachment
                var emailSubject = scheduledReport.EmailSubject ?? $"Scheduled Report: {scheduledReport.ScheduleName}";
                var emailBody = scheduledReport.EmailBody ?? $"Please find the attached report: {scheduledReport.ScheduleName}";

                await SendReportEmailAsync(
                    recipients,
                    emailSubject,
                    emailBody,
                    reportBytes,
                    scheduledReport.OutputFormat,
                    scheduledReport.ScheduleName
                );

                // Update scheduled report
                scheduledReport.LastRunDate = _dateTimeProvider.Now;
                scheduledReport.NextRunDate = CalculateNextRunDate(
                    scheduledReport.Frequency,
                    scheduledReport.TimeOfDay,
                    scheduledReport.DayOfWeek,
                    scheduledReport.DayOfMonth
                );
                scheduledReport.ModificationDate = _dateTimeProvider.Now;

                await _scheduledReportRepository.UpdateAsync(scheduledReport);

                await SaveExecutionAsync(execution);

                _logger.LogInformation("Successfully executed scheduled report {Id}", scheduledReportId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error executing scheduled report {Id}", scheduledReportId);
                execution.Status = "Failed";
                execution.ErrorMessage = ex.Message;
                await SaveExecutionAsync(execution);
            }
        }

        private async Task<byte[]?> GenerateReportBytesAsync(string reportUrl, string outputFormat)
        {
            XtraReport? report = null;
            try
            {
                _logger.LogInformation("Generating report {ReportUrl} in format {Format}", reportUrl, outputFormat);

                // Get report data from database
                var reportResult = await _reportService.GetByUrlAsync(reportUrl);
                if (!reportResult.Succeeded || reportResult.Data == null)
                {
                    _logger.LogWarning("Report not found: {ReportUrl}", reportUrl);
                    return null;
                }

                // Load report from database layout data
                if (reportResult.Data.LayoutData != null && reportResult.Data.LayoutData.Length > 0)
                {
                    using var layoutStream = new MemoryStream(reportResult.Data.LayoutData);
                    report = new XtraReport();
                    report.LoadLayoutFromXml(layoutStream);
                }
                else
                {
                    // Fallback to factory if no layout data
                    report = _reportFactory.Create(reportUrl);
                }

                // Export report to memory stream based on format
                using var memoryStream = new MemoryStream();
                
                if (outputFormat.Equals("PDF", StringComparison.OrdinalIgnoreCase))
                {
                    report.ExportToPdf(memoryStream);
                }
                else if (outputFormat.Equals("Excel", StringComparison.OrdinalIgnoreCase) || 
                         outputFormat.Equals("XLSX", StringComparison.OrdinalIgnoreCase))
                {
                    report.ExportToXlsx(memoryStream);
                }
                else
                {
                    _logger.LogWarning("Unsupported output format: {Format}. Defaulting to PDF.", outputFormat);
                    report.ExportToPdf(memoryStream);
                }

                var reportBytes = memoryStream.ToArray();
                _logger.LogInformation("Successfully generated report {ReportUrl} in format {Format}. Size: {Size} bytes", 
                    reportUrl, outputFormat, reportBytes.Length);

                return reportBytes;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error generating report from URL {ReportUrl}", reportUrl);
                return null;
            }
            finally
            {
                // Dispose report to free resources
                report?.Dispose();
            }
        }

        private async Task SendReportEmailAsync(
            List<string> recipients,
            string subject,
            string body,
            byte[] reportBytes,
            string format,
            string reportName)
        {
            try
            {
                _logger.LogInformation("Sending report email to {Count} recipients. Report: {ReportName}, Format: {Format}, Size: {Size} bytes", 
                    recipients.Count, reportName, format, reportBytes?.Length ?? 0);

                if (recipients == null || !recipients.Any())
                {
                    _logger.LogWarning("No recipients specified for report email");
                    return;
                }

                // Determine file extension and content type based on format
                string fileExtension = format.Equals("Excel", StringComparison.OrdinalIgnoreCase) || 
                                      format.Equals("XLSX", StringComparison.OrdinalIgnoreCase) 
                    ? "xlsx" 
                    : "pdf";
                
                string contentType = fileExtension == "xlsx" 
                    ? "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet" 
                    : "application/pdf";

                string attachmentFileName = $"{reportName}_{_dateTimeProvider.Now:yyyyMMdd_HHmmss}.{fileExtension}";

                // Send email to each recipient
                var tasks = recipients.Select(async recipient =>
                {
                    try
                    {
                        await _emailSender.SendEmailWithAttachmentAsync(
                            recipient,
                            subject,
                            body,
                            reportBytes,
                            attachmentFileName,
                            contentType
                        );
                        _logger.LogInformation("Report email sent successfully to {Recipient}", recipient);
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "Failed to send report email to {Recipient}", recipient);
                        // Continue with other recipients even if one fails
                    }
                });

                await Task.WhenAll(tasks);
                _logger.LogInformation("Completed sending report emails to all recipients");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error sending report email");
                throw;
            }
        }

        private async Task SaveExecutionAsync(ScheduledReportExecution execution)
        {
            try
            {
                await _executionRepository.AddAsync(execution);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error saving execution record");
            }
        }

        private DateTime? CalculateNextRunDate(string frequency, string timeOfDay, int? dayOfWeek, int? dayOfMonth)
        {
            if (!TimeSpan.TryParse(timeOfDay, out var time))
            {
                time = new TimeSpan(8, 0, 0);
            }

            var now = _dateTimeProvider.Now;
            var today = now.Date;
            var targetTime = today.Add(time);

            if (targetTime <= now)
            {
                targetTime = targetTime.AddDays(1);
            }

            switch (frequency.ToLower())
            {
                case "daily":
                    return targetTime;

                case "weekly":
                    if (dayOfWeek.HasValue)
                    {
                        var currentDayOfWeek = (int)targetTime.DayOfWeek;
                        var daysUntilTarget = (dayOfWeek.Value - currentDayOfWeek + 7) % 7;
                        if (daysUntilTarget == 0 && targetTime <= now)
                        {
                            daysUntilTarget = 7;
                        }
                        return targetTime.AddDays(daysUntilTarget);
                    }
                    return targetTime;

                case "monthly":
                    if (dayOfMonth.HasValue)
                    {
                        var day = Math.Min(dayOfMonth.Value, DateTime.DaysInMonth(targetTime.Year, targetTime.Month));
                        var targetDate = new DateTime(targetTime.Year, targetTime.Month, day);
                        if (targetDate < now.Date)
                        {
                            targetDate = targetDate.AddMonths(1);
                            day = Math.Min(dayOfMonth.Value, DateTime.DaysInMonth(targetDate.Year, targetDate.Month));
                            targetDate = new DateTime(targetDate.Year, targetDate.Month, day);
                        }
                        return targetDate.Add(time);
                    }
                    return targetTime;

                default:
                    return targetTime;
            }
        }
    }
}
