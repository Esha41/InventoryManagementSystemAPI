using DevExpress.XtraReports.UI;
using DevExpress.XtraReports.Web.Extensions;
using Ettad.Application.Common.Interfaces;
using Ettad.Data.Enums;
using Ettad.Reporting.Services;
using Ettad.Reporting.Services.Reports.Dtos;

namespace Ettad.Reporting.Storage
{
    /// <summary>
    /// Custom report storage extension that saves reports to the database using IReportService
    /// </summary>
    public class CustomReportStorageWebExtension : ReportStorageWebExtension
    {
        private readonly IReportService _reportService;

        public CustomReportStorageWebExtension(IReportService reportService)
        {
            _reportService = reportService;
        }

        public override bool IsValidUrl(string url)
        {
            // Validate report URL format
            return !string.IsNullOrEmpty(url) &&
                   !url.Contains("..") &&
                   url.Length < 500; // Reasonable length limit
        }
        public override bool CanSetData(string url)
        {
            return true;
        }

        public override byte[] GetData(string url)
        {
            try
            {
                // If it's a base template, return it directly
                if (url == "BaseReportTemplate")
                {
                    var baseReport = new Ettad.Reporting.Reports.BaseReportTemplate();
                    using (var ms = new MemoryStream())
                    {
                        baseReport.SaveLayoutToXml(ms);
                        return ms.ToArray();
                    }
                }

                // Try to get report from database using service
                var result = _reportService.GetByUrlAsync(url).GetAwaiter().GetResult();

                if (result.Succeeded && result.Data != null)
                {
                    return result.Data.LayoutData ?? Array.Empty<byte>();
                }

                return Array.Empty<byte>();
            }
            catch (Exception ex)
            {
                // Log error
                System.Diagnostics.Debug.WriteLine($"Error getting report data: {ex.Message}");
                return Array.Empty<byte>();
            }
        }

        public override Dictionary<string, string> GetUrls()
        {
            try
            {

                // Get all reports using service
                var result = _reportService.GetAllAsync().GetAwaiter().GetResult();

                var urls = new Dictionary<string, string>();

                if (result.Succeeded && result.Data != null)
                {
                    foreach (var report in result.Data)
                    {
                        urls[report.Url] = report.ReportName;
                    }
                }

                urls["BaseReportTemplate"] = "Base Report Template";
                return urls;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error getting report URLs: {ex.Message}");
                return new Dictionary<string, string>();
            }
        }

        public override void SetData(XtraReport report, string url)
        {
            if(report.Name!=report.DisplayName)
                report.DisplayName = report.Name;

            var reportName = report.DisplayName;

            // Save report layout to memory stream
            byte[] layoutData;
            using (var ms = new MemoryStream())
            {
                report.SaveLayoutToXml(ms);
                layoutData = ms.ToArray();
            }

            // Check if report exists using service
            var existingReportResult = _reportService.GetByUrlAsync(url).GetAwaiter().GetResult();

            if (existingReportResult.Succeeded && existingReportResult.Data != null)
            {
                // Update existing report
                var updateDto = new UpdateReportDto
                {
                    ReportName = reportName ?? url,
                    ReportStatusId = existingReportResult.Data.ReportStatusId, // Keep existing status
                    Url = url,
                    Description = existingReportResult.Data.Description,
                    LayoutData = layoutData,
                    ReportParameters = existingReportResult.Data.ReportParameters,
                    IsTemplate = existingReportResult.Data.IsTemplate,
                    IsPublic = existingReportResult.Data.IsPublic
                };

                var updateResult = _reportService.UpdateAsync(existingReportResult.Data.Id, updateDto).GetAwaiter().GetResult();

                if (!updateResult.Succeeded)
                {
                    throw new Exception($"Failed to update report: {updateResult.Message}");
                }
            }
        }
        private bool ReportExists(string name)
        {
            return _reportService.IsReportExists(name).GetAwaiter().GetResult();
        }

        public override string SetNewData(XtraReport report, string defaultUrl)
        {
            try
            {
                // Generate unique URL if default URL already exists
                var url = defaultUrl;
                var counter = 1;
                var reportName = report.DisplayName;
                report.Name = report.DisplayName;

                if (ReportExists(reportName))
                    throw new Exception($"Report with name '{reportName}' already exists.");

                // Check if URL exists using service
                var checkResult = _reportService.GetByUrlAsync(url).GetAwaiter().GetResult();
                while (checkResult.Succeeded && checkResult.Data != null)
                {
                    url = $"{defaultUrl}_{counter}";
                    counter++;
                    checkResult = _reportService.GetByUrlAsync(url).GetAwaiter().GetResult();
                }

                // Save report layout
                byte[] layoutData;
                using (var ms = new MemoryStream())
                {
                    report.SaveLayoutToXml(ms);
                    layoutData = ms.ToArray();
                }

                // Create new report using service
                var createDto = new CreateReportDto
                {
                    ReportName = reportName ?? url,
                    ReportStatusId = (int)ReportStatuses.Draft, // Default to Draft
                    Url = url,
                    LayoutData = layoutData,
                    IsTemplate = false,
                    IsPublic = false
                };

                var createResult = _reportService.CreateAsync(createDto).GetAwaiter().GetResult();

                if (!createResult.Succeeded)
                {
                    throw new Exception($"Failed to create report: {createResult.Message}");
                }

                return url;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error creating new report: {ex.Message}");
                throw;
            }
        }
    }
}
