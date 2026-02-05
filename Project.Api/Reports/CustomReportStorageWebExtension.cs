using DevExpress.XtraReports.UI;
using DevExpress.XtraReports.Web.ClientControls;
using DevExpress.XtraReports.Web.Extensions;
using Ettad.Data.Enums;
using Ettad.Inventory.Service.AllowanceItems;
using Ettad.Reporting.Services;
using Ettad.Reporting.Services.Reports.Dtos;
using Project.Api.Reports.DataSources;
using Project.Api.Reports.Factories;

namespace Ettad.Reporting.Storage
{
    /// <summary>
    /// Custom report storage extension that saves reports to the database using IReportService
    /// </summary>
    public class CustomReportStorageWebExtension : ReportStorageWebExtension
    {
        private readonly IReportService _reportService;
        private readonly ReportFactory _reportFactory;

        private readonly IServiceScopeFactory _scopeFactory;
        public CustomReportStorageWebExtension(IServiceScopeFactory scopeFactory, IReportService reportService, ReportFactory reportFactory)
        {
            _reportService = reportService;
            _reportFactory = reportFactory;
            _scopeFactory = scopeFactory;
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
            { // 1?? Try DB first
              var result = _reportService.GetByUrlAsync(url).GetAwaiter().GetResult(); 
                if (result.Succeeded && result.Data?.LayoutData != null) return result.Data.LayoutData; 
                
                // 2?? No layout in DB ? create via factory
                var report = _reportFactory.Create(url); 
                using var ms = new MemoryStream(); 
                report.SaveLayoutToXml(ms); 
                return ms.ToArray(); 
            } 
            catch (FaultException ex) 
            { 
                // Log error
                System.Diagnostics.Debug.WriteLine($"Error getting report data: {ex.Message}"); return Array.Empty<byte>(); } 
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
            catch (FaultException ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error getting report URLs: {ex.Message}");
                return new Dictionary<string, string>();
            }
        }

        public override void SetData(XtraReport report, string url)
        {
            if (string.IsNullOrWhiteSpace(report.DisplayName))
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
                    ReportParameters = existingReportResult.Data.ReportParameters
                };

                var updateResult = _reportService.UpdateAsync(existingReportResult.Data.Id, updateDto).GetAwaiter().GetResult();

                if (!updateResult.Succeeded)
                {
                    throw new FaultException($"Failed to update report: {updateResult.Message}");
                }
            }
        }
        private bool ReportExists(string name)
        {
            return _reportService.IsReportExists(name).GetAwaiter().GetResult();
        }

        public override string SetNewData(XtraReport report, string defaultUrl)
        {
            // Generate unique URL if default URL already exists
            var url = defaultUrl;
            var counter = 1;
            var reportName = report.DisplayName;

            if (ReportExists(reportName))
                throw new FaultException("Report with this name already exists, Try another.");

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
                LayoutData = layoutData
            };

            var createResult = _reportService.CreateAsync(createDto).GetAwaiter().GetResult();

            if (!createResult.Succeeded)
            {
                throw new FaultException(createResult.Message);
            }

            return createResult.Data ?? url;
        }
    }
}
