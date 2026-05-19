using DevExpress.XtraReports.UI;
using Ettad.CrossCutting.Comman.Time;
using Ettad.ReportManagement.Service.Interfaces;
using Ettad.User.Services.Helpers;
using Microsoft.Extensions.Logging;

namespace Ettad.ReportManagement.Service.Services
{
    public class ScheduledReportExportMailHelper : IScheduledReportExportMailHelper
    {
        private readonly IReportService _reportService;
        private readonly IEmailSender _emailSender;
        private readonly IDateTimeProvider _dateTimeProvider;
        private readonly ILogger<ScheduledReportExportMailHelper> _logger;

        public ScheduledReportExportMailHelper(
            IReportService reportService,
            IEmailSender emailSender,
            IDateTimeProvider dateTimeProvider,
            ILogger<ScheduledReportExportMailHelper> logger)
        {
            _reportService = reportService;
            _emailSender = emailSender;
            _dateTimeProvider = dateTimeProvider;
            _logger = logger;
        }

        public async Task<byte[]?> GetLayoutBytesAsync(string reportUrl, CancellationToken cancellationToken = default)
        {
            try
            {
                var response = await _reportService.GetByUrlAsync(reportUrl).ConfigureAwait(false);
                if (response?.Succeeded != true || response.Data?.LayoutData is not { Length: > 0 } data)
                    return null;
                return data;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "GetByUrlAsync failed {ReportUrl}", reportUrl);
                return null;
            }
        }

        public async Task<byte[]?> RenderReportAsync(string reportUrl, string format, CancellationToken cancellationToken = default)
        {
            try
            {
                var layout = await GetLayoutBytesAsync(reportUrl, cancellationToken).ConfigureAwait(false);
                if (layout == null)
                {
                    _logger.LogWarning("Report not found or empty layout {ReportUrl}", reportUrl);
                    return null;
                }

                using var report = new XtraReport();
                using (var stream = new MemoryStream(layout))
                    report.LoadLayoutFromXml(stream);

                using var outStream = new MemoryStream();
                Export(report, outStream, format);
                var bytes = outStream.ToArray();
                _logger.LogInformation("Rendered {ReportUrl} as {Format} ({Size} B)", reportUrl, format, bytes.Length);
                return bytes;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "RenderReportAsync {ReportUrl}", reportUrl);
                return null;
            }
        }

        public Task<byte[]?> RenderReportFromLayoutAsync(byte[] layout, string format, Action<XtraReport>? configureReport, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            try
            {
                using var report = new XtraReport();
                using (var ms = new MemoryStream(layout))
                    report.LoadLayoutFromXml(ms);

                configureReport?.Invoke(report);

                using var destination = new MemoryStream();
                Export(report, destination, format);
                return Task.FromResult<byte[]?>(destination.ToArray());
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "RenderReportFromLayoutAsync failed");
                return Task.FromResult<byte[]?>(null);
            }
        }

        public async Task SendEmailsWithAttachmentAsync( IReadOnlyList<string> recipients, string subject, string body, byte[] reportBytes,
            string format,
            string scheduleName,
            CancellationToken cancellationToken = default)
        {
            if (recipients.Count == 0)
            {
                _logger.LogWarning("No recipients for attachment email");
                return;
            }

            var (ext, contentType) = IsSpreadsheet(format)
                ? ("xlsx", "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet")
                : ("pdf", "application/pdf");

            var fileName = $"{scheduleName}_{_dateTimeProvider.Now:yyyyMMdd_HHmmss}.{ext}";
            _logger.LogInformation(
                "Email report to {Count} recipient(s): {Schedule}, {Size} B",
                recipients.Count,
                scheduleName,
                reportBytes.Length);

            var tasks = recipients.Select(async recipient =>
            {
                cancellationToken.ThrowIfCancellationRequested();
                try
                {
                    await _emailSender.SendEmailWithAttachmentAsync(
                        recipient, subject, body, reportBytes, fileName, contentType).ConfigureAwait(false);
                    _logger.LogInformation("Sent to {Recipient}", recipient);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Send failed {Recipient}", recipient);
                }
            });

            await Task.WhenAll(tasks).ConfigureAwait(false);
        }

        private static bool IsSpreadsheet(string format) =>
            format.Equals("Excel", StringComparison.OrdinalIgnoreCase)
            || format.Equals("XLSX", StringComparison.OrdinalIgnoreCase);

        private void Export(XtraReport report, MemoryStream destination, string format)
        {
            if (IsSpreadsheet(format))
                report.ExportToXlsx(destination);
            else
            {
                if (!format.Equals("PDF", StringComparison.OrdinalIgnoreCase))
                    _logger.LogWarning("Unsupported format {Format}, using PDF.", format);
                report.ExportToPdf(destination);
            }
        }
    }
}
