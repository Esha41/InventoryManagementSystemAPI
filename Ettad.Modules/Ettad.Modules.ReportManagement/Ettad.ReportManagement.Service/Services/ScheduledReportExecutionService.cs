using Ettad.CrossCutting.Comman.Time;
using Ettad.Data.Entities;
using Ettad.Data.Interfaces.Repositories;
using Ettad.ReportManagement.Service.Interfaces;
using Ettad.ReportManagement.Service.Services.ScheduledReportDistribution;
using Ettad.User.Services.Interfaces;
using Microsoft.Extensions.Logging;

namespace Ettad.ReportManagement.Service.Services
{
    public class ScheduledReportExecutionService : IScheduledReportExecutionService
    {
        private const string Success = "Success";
        private const string Failed = "Failed";

        private readonly ICrossCuttingRepository<ScheduledReport> _scheduledReportRepository;
        private readonly ICrossCuttingRepository<ScheduledReportExecution> _executionRepository;
        private readonly IDateTimeProvider _dateTimeProvider;
        private readonly ILogger<ScheduledReportExecutionService> _logger;
        private readonly IScheduledReportExportMailHelper _exportMailHelper;
        private readonly IUserService _userService;
        private readonly IScheduledReportDistributionStrategy _auditorPendingApprovalsExecutionService;

        public ScheduledReportExecutionService(
            ICrossCuttingRepository<ScheduledReport> scheduledReportRepository,
            ICrossCuttingRepository<ScheduledReportExecution> executionRepository,
            IDateTimeProvider dateTimeProvider,
            ILogger<ScheduledReportExecutionService> logger,
            IScheduledReportExportMailHelper exportMailHelper,
            IUserService userService,
            IScheduledReportDistributionStrategy auditorPendingApprovalsExecutionService)
        {
            _scheduledReportRepository = scheduledReportRepository;
            _executionRepository = executionRepository;
            _dateTimeProvider = dateTimeProvider;
            _logger = logger;
            _exportMailHelper = exportMailHelper;
            _userService = userService;
            _auditorPendingApprovalsExecutionService = auditorPendingApprovalsExecutionService;
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
                          && sr.NextRunDate!.Value <= now,
                    false,
                    nameof(ScheduledReport.Report),
                    nameof(ScheduledReport.Recipients));

                foreach (var item in scheduledReports)
                {
                    try
                    {
                        await ExecuteReportAsync(item.Id);
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "Error executing scheduled report {Id}", item.Id);
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

            var execution = NewExecution(scheduledReportId);
            var cancellationToken = CancellationToken.None;

            try
            {
                var sr = await _scheduledReportRepository.FindOneAsync(
                    x => x.Id == scheduledReportId && !x.IsDeleted,
                    false,
                    nameof(ScheduledReport.Report),
                    nameof(ScheduledReport.Recipients));

                if (sr == null || !sr.IsActive)
                {
                    _logger.LogWarning("Scheduled report {Id} not found or inactive", scheduledReportId);
                    return;
                }

                if (sr.Report == null)
                {
                    Fail(execution, "Report not found");
                    await PersistExecutionAsync(execution);
                    return;
                }

                //pending auditor requests dynamic recipients scheduler
                var strategy = _auditorPendingApprovalsExecutionService.CanHandle(sr);
                if (strategy)
                {
                    await _auditorPendingApprovalsExecutionService.ExecuteAsync(sr, execution, cancellationToken).ConfigureAwait(false);
                    if (execution.Status == Success)
                        await BumpScheduleAsync(sr);
                    await PersistExecutionAsync(execution);
                    return;
                }

                var to = await ResolveRecipientEmailsAsync(sr.Recipients);
                if (to.Count == 0)
                {
                    Fail(execution, "No recipients configured");
                    await PersistExecutionAsync(execution);
                    return;
                }

                var bytes = await _exportMailHelper
                    .RenderReportAsync(sr.Report.Url, sr.OutputFormat, cancellationToken)
                    .ConfigureAwait(false);
                if (bytes == null || bytes.Length == 0)
                {
                    Fail(execution, "Failed to generate report");
                    await PersistExecutionAsync(execution);
                    return;
                }

                execution.FileSizeBytes = bytes.Length;
                execution.RecipientCount = to.Count;

                var subject = sr.EmailSubject ?? $"Scheduled Report: {sr.ScheduleName}";
                var body = sr.EmailBody ?? $"Please find the attached report: {sr.ScheduleName}";

                await _exportMailHelper
                    .SendEmailsWithAttachmentAsync(to, subject, body, bytes, sr.OutputFormat, sr.ScheduleName, cancellationToken)
                    .ConfigureAwait(false);
                await BumpScheduleAsync(sr);
                await PersistExecutionAsync(execution);

                _logger.LogInformation("Successfully executed scheduled report {Id}", scheduledReportId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error executing scheduled report {Id}", scheduledReportId);
                execution.Status = Failed;
                execution.ErrorMessage = ex.Message;
                await PersistExecutionAsync(execution);
            }
        }

        private ScheduledReportExecution NewExecution(Guid scheduledReportId)
        {
            var now = _dateTimeProvider.Now;
            return new ScheduledReportExecution
            {
                Id = Guid.NewGuid(),
                ScheduledReportId = scheduledReportId,
                ExecutionDate = now,
                Status = Success,
                CreationDate = now
            };
        }

        private static void Fail(ScheduledReportExecution e, string message)
        {
            e.Status = Failed;
            e.ErrorMessage = message;
        }

        private async Task BumpScheduleAsync(ScheduledReport sr)
        {
            sr.LastRunDate = _dateTimeProvider.Now;
            sr.NextRunDate = NextRun(sr.Frequency, sr.TimeOfDay, sr.DayOfWeek, sr.DayOfMonth);
            sr.ModificationDate = _dateTimeProvider.Now;
            await _scheduledReportRepository.UpdateAsync(sr);
        }

        private async Task<List<string>> ResolveRecipientEmailsAsync(IEnumerable<ScheduledReportRecipient>? recipients)
        {
            if (recipients == null)
                return [];

            var list = new List<string>();
            foreach (var r in recipients.Where(x => !x.IsDeleted))
            {
                string? email = null;
                if (!string.IsNullOrWhiteSpace(r.UserId))
                {
                    try
                    {
                        var ur = await _userService.GetByIdAsync(r.UserId);
                        if (ur.Succeeded && ur.Data != null && !string.IsNullOrWhiteSpace(ur.Data.Email))
                            email = ur.Data.Email;
                        else
                            _logger.LogWarning("User {UserId} not found or no email", r.UserId);
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "Resolve email for user {UserId}", r.UserId);
                    }
                }
                else
                {
                    email = r.EmailAddress;
                }

                if (!string.IsNullOrWhiteSpace(email))
                    list.Add(email);
            }

            return list.Distinct(StringComparer.OrdinalIgnoreCase).ToList();
        }

        private async Task PersistExecutionAsync(ScheduledReportExecution execution)
        {
            try
            {
                await _executionRepository.AddAsync(execution);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Save execution failed");
            }
        }

        private DateTime? NextRun(string frequency, string timeOfDay, int? dayOfWeek, int? dayOfMonth)
        {
            if (!TimeSpan.TryParse(timeOfDay, out var time))
                time = new TimeSpan(8, 0, 0);

            var now = _dateTimeProvider.Now;
            var today = now.Date;
            var targetTime = today.Add(time);

            if (targetTime <= now)
                targetTime = targetTime.AddDays(1);

            switch (frequency.ToLower())
            {
                case "daily":
                    return targetTime;
                case "weekly":
                    if (dayOfWeek.HasValue)
                    {
                        var current = (int)targetTime.DayOfWeek;
                        var delta = (dayOfWeek.Value - current + 7) % 7;
                        if (delta == 0 && targetTime <= now)
                            delta = 7;
                        return targetTime.AddDays(delta);
                    }
                    return targetTime;
                case "monthly":
                    if (!dayOfMonth.HasValue)
                        return targetTime;
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
                default:
                    return targetTime;
            }
        }
    }
}
