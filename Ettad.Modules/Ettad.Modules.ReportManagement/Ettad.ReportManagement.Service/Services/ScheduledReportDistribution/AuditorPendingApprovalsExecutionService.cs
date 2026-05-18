using System.Data;
using Ettad.Data.Constants;
using Ettad.Data.Entities;
using DevExpress.XtraReports.Parameters;
using Ettad.EntityFramework.DataBaseContext;
using Ettad.ReportManagement.Service.Dtos;
using Ettad.ReportManagement.Service.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Ettad.ReportManagement.Service.Services.ScheduledReportDistribution
{
    public class AuditorPendingApprovalsExecutionService : IScheduledReportDistributionStrategy
    {
        private const string Success = "Success";
        private const string Failed = "Failed";

        private readonly ApplicationDbContext _dbContext;
        private readonly IScheduledReportExportMailHelper _exportMailHelper;
        private readonly ILogger<AuditorPendingApprovalsExecutionService> _logger;

        public AuditorPendingApprovalsExecutionService(
            ApplicationDbContext dbContext,
            IScheduledReportExportMailHelper exportMailHelper,
            ILogger<AuditorPendingApprovalsExecutionService> logger)
        {
            _dbContext = dbContext;
            _exportMailHelper = exportMailHelper;
            _logger = logger;
        }

        public bool CanHandle(ScheduledReport scheduledReport)
        {
            var templateName = scheduledReport.Report?.TemplateName;
            return string.Equals(
                templateName,
                ReportConstants.AuditorPendingApprovalsReportTemplate,
                StringComparison.OrdinalIgnoreCase);
        }

        public async Task ExecuteAsync(
            ScheduledReport scheduledReport,
            ScheduledReportExecution execution,
            CancellationToken cancellationToken)
        {
            execution.FileSizeBytes = null;

            var layout = await _exportMailHelper
                .GetLayoutBytesAsync(scheduledReport.Report!.Url, cancellationToken)
                .ConfigureAwait(false);
            if (layout == null)
            {
                Fail(execution, "Report layout not found or empty");
                return;
            }

            List<AuditorPendingApprovalReportRow> rows;
            try
            {
                rows = await ReadAuditorPendingRowsAsync(_dbContext, cancellationToken).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "dbo.GetAuditorPendingApprovals failed");
                Fail(execution, "Stored procedure execution failed");
                return;
            }

            if (rows.Count == 0)
            {
                execution.Status = Success;
                execution.RecipientCount = 0;
                execution.ErrorMessage = null;
                _logger.LogInformation("Auditor pending: zero rows.");
                return;
            }

            var groups = rows
                .Where(r => !string.IsNullOrWhiteSpace(r.NextApprover))
                .GroupBy(r => r.NextApprover!.Trim())
                .Where(g => g.Key.Length > 0)
                .ToList();

            if (groups.Count == 0)
            {
                Fail(execution, "No NextApprover labels in result set — cannot resolve recipients.");
                execution.RecipientCount = 0;
                return;
            }

            var subjectBase = scheduledReport.EmailSubject ?? $"Scheduled Report: {scheduledReport.ScheduleName}";
            var bodyBase = scheduledReport.EmailBody ?? $"Please find the attached report: {scheduledReport.ScheduleName}";

            var sentCount = 0;
            var anySkipped = false;

            foreach (var group in groups)
            {
                cancellationToken.ThrowIfCancellationRequested();

                var label = group.First().NextApprover!.Trim();
                var emails = group
                    .SelectMany(static r => ParseNextApproverUserEmails(r.NextApproverUserEmails))
                    .Distinct(StringComparer.OrdinalIgnoreCase)
                    .ToList();
                if (emails.Count == 0)
                {
                    anySkipped = true;
                    _logger.LogWarning(
                        "Skipping group — NextApproverUserEmails empty for ({Preview})",
                        label.Length > 200 ? label[..200] + "…" : label);
                    continue;
                }

                var slice = await RenderAuditorSliceAsync(layout, group.ToList(), scheduledReport.OutputFormat, cancellationToken)
                    .ConfigureAwait(false);
                if (slice == null || slice.Length == 0)
                {
                    anySkipped = true;
                    _logger.LogError("Export failed for NextApprover slice (len={Len})", label.Length);
                    continue;
                }

                var fileTag = SafeFileFragment(label);
                if (string.IsNullOrEmpty(fileTag))
                    fileTag = "NextApprover";

                foreach (var email in emails)
                {
                    await _exportMailHelper.SendEmailsWithAttachmentAsync(
                            new List<string> { email },
                            $"{subjectBase} [{fileTag}]",
                            bodyBase,
                            slice,
                            scheduledReport.OutputFormat,
                            scheduledReport.ScheduleName,
                            cancellationToken)
                        .ConfigureAwait(false);
                    sentCount++;
                }

                execution.RecipientCount = sentCount;
            }

            if (sentCount == 0)
            {
                Fail(execution,
                    anySkipped
                        ? "Could not send to any recipients — NextApproverUserEmails missing or invalid for every group."
                        : "Unexpected: no outgoing emails.");
                return;
            }

            execution.Status = Success;
            execution.ErrorMessage = anySkipped
                ? "Some groups were skipped — see logs. At least one emailed successfully."
                : null;
        }

        private static void Fail(ScheduledReportExecution e, string message)
        {
            e.Status = Failed;
            e.ErrorMessage = message;
        }

        private static async Task<List<AuditorPendingApprovalReportRow>> ReadAuditorPendingRowsAsync(
            ApplicationDbContext db,
            CancellationToken cancellationToken)
        {
            var connection = db.Database.GetDbConnection();
            await db.Database.OpenConnectionAsync(cancellationToken).ConfigureAwait(false);

            try
            {
                await using var command = connection.CreateCommand();
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "dbo.GetAuditorPendingApprovals";
                command.CommandTimeout = 180;

                await using var reader = await command.ExecuteReaderAsync(cancellationToken).ConfigureAwait(false);

                var oRequestId = reader.GetOrdinal("RequestId");
                var oOrderId = reader.GetOrdinal("OrderId");
                var oRequestDate = reader.GetOrdinal("RequestDate");
                var oPriority = reader.GetOrdinal("Priority");
                var oRequestType = reader.GetOrdinal("RequestType");
                var oStatus = reader.GetOrdinal("Status");
                var oPendingFrom = reader.GetOrdinal("PendingFrom");
                var oPreviousApprover = reader.GetOrdinal("PreviousApprover");
                var oPendingBy = reader.GetOrdinal("PendingBy");
                var oNextApprover = reader.GetOrdinal("NextApprover");
                var oNextApproverUserEmails = reader.GetOrdinal("NextApproverUserEmails");

                var list = new List<AuditorPendingApprovalReportRow>();
                while (await reader.ReadAsync(cancellationToken).ConfigureAwait(false))
                {
                    list.Add(new AuditorPendingApprovalReportRow
                    {
                        RequestId = reader.GetInt64(oRequestId),
                        OrderId = reader.IsDBNull(oOrderId) ? string.Empty : reader.GetString(oOrderId),
                        RequestDate = reader.GetDateTime(oRequestDate),
                        Priority = reader.IsDBNull(oPriority) ? string.Empty : reader.GetString(oPriority),
                        RequestType = reader.IsDBNull(oRequestType) ? string.Empty : reader.GetString(oRequestType),
                        Status = reader.IsDBNull(oStatus) ? string.Empty : reader.GetString(oStatus),
                        PendingFrom = reader.GetDateTime(oPendingFrom),
                        PreviousApprover = reader.IsDBNull(oPreviousApprover) ? null : reader.GetString(oPreviousApprover),
                        PendingBy = reader.IsDBNull(oPendingBy) ? null : reader.GetString(oPendingBy),
                        NextApprover = reader.IsDBNull(oNextApprover) ? null : reader.GetString(oNextApprover),
                        NextApproverUserEmails = reader.IsDBNull(oNextApproverUserEmails)
                            ? null
                            : reader.GetString(oNextApproverUserEmails)
                    });
                }

                return list;
            }
            finally
            {
                await db.Database.CloseConnectionAsync().ConfigureAwait(false);
            }
        }

        private static IEnumerable<string> ParseNextApproverUserEmails(string? nextApproverUserEmails)
        {
            if (string.IsNullOrWhiteSpace(nextApproverUserEmails))
                yield break;

            foreach (var part in nextApproverUserEmails.Split(
                         ';',
                         StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries))
            {
                if (!string.IsNullOrWhiteSpace(part))
                    yield return part.Trim();
            }
        }

        private Task<byte[]?> RenderAuditorSliceAsync(
            byte[] layoutData,
            IReadOnlyList<AuditorPendingApprovalReportRow> rows,
            string outputFormat,
            CancellationToken cancellationToken)
        {
            return _exportMailHelper.RenderReportFromLayoutAsync(
                layoutData,
                outputFormat,
                report =>
                {
                    report.DataSource = rows;
                    report.DataMember = string.Empty;

                    foreach (Parameter parameter in report.Parameters)
                    {
                        if (parameter.Name != "Language")
                            continue;
                        if (parameter.Value is null || parameter.Value is string s && string.IsNullOrWhiteSpace(s))
                            parameter.Value = "en";
                    }
                },
                cancellationToken);
        }

        private static string SafeFileFragment(string label)
        {
            var trimmed = label.Trim();
            var invalid = Path.GetInvalidFileNameChars();
            var buffer = trimmed.ToCharArray();
            for (var i = 0; i < buffer.Length; i++)
            {
                if (invalid.Contains(buffer[i]))
                    buffer[i] = '_';
                if (buffer[i] == '|')
                    buffer[i] = '_';
            }

            var s = new string(buffer);
            return s.Length <= 96 ? s : s[..96];
        }
    }
}
