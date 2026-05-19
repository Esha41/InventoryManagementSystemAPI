using DevExpress.XtraReports.UI;

namespace Ettad.ReportManagement.Service.Interfaces
{
    public interface IScheduledReportExportMailHelper
    {
        Task<byte[]?> GetLayoutBytesAsync(string reportUrl, CancellationToken cancellationToken = default);

        Task<byte[]?> RenderReportAsync(string reportUrl, string format, CancellationToken cancellationToken = default);

        Task<byte[]?> RenderReportFromLayoutAsync(
            byte[] layout,
            string format,
            Action<XtraReport>? configureReport,
            CancellationToken cancellationToken = default);

        Task SendEmailsWithAttachmentAsync(
            IReadOnlyList<string> recipients,
            string subject,
            string body,
            byte[] reportBytes,
            string format,
            string scheduleName,
            CancellationToken cancellationToken = default);
    }
}
