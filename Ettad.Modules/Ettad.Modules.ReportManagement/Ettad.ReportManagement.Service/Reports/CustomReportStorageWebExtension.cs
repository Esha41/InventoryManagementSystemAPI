using DevExpress.XtraReports.UI;
using DevExpress.XtraReports.Web.ClientControls;
using DevExpress.XtraReports.Web.Extensions;
using Ettad.Data.Enums;
using Ettad.ReportManagement.Service.Reports.Factories;
using Ettad.ReportManagement.Service.Dtos;
using Ettad.ReportManagement.Service.Interfaces;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.DependencyInjection;

namespace Ettad.ReportManagement.Service.Reports
{
    /// <summary>
    /// Custom report storage extension that saves reports to the database using IReportService
    /// </summary>
    public class CustomReportStorageWebExtension : ReportStorageWebExtension
    {
        private readonly IReportService _reportService;
        private readonly ReportFactory _reportFactory;

        public CustomReportStorageWebExtension(IServiceScopeFactory scopeFactory, IReportService reportService, ReportFactory reportFactory)
        {
            _reportService = reportService;
            _reportFactory = reportFactory;
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
                // Extract departmentId(s) from url
                List<long> departmentIds = new List<long>();
                bool? superAdminFromUrl = null;

                var parts = url.Split('?');
                var baseUrl = parts[0];
                if (parts.Length > 1)
                {
                    var queryParams = QueryHelpers.ParseQuery(parts[1]);

                    if (queryParams.TryGetValue("departmentId", out var value))
                    {
                        var values = value.ToString().Split(',', StringSplitOptions.RemoveEmptyEntries);
                        foreach (var deptValue in values)
                        {
                            if (int.TryParse(deptValue.Trim(), out var parsedId))
                                departmentIds.Add(parsedId);
                        }
                    }

                    // Extract superadmin parameter from URL
                    if (queryParams.TryGetValue("superadmin", out var superAdminValue))
                    {
                        if (bool.TryParse(superAdminValue.ToString(), out var parsedSuperAdmin))
                            superAdminFromUrl = parsedSuperAdmin;
                    }

                    //if user is not super admin but has NULL departmentID, don't display any data
                    if ((superAdminFromUrl == null || superAdminFromUrl == false) && departmentIds.Count == 0)
                        departmentIds.Add(-1);
                }

                XtraReport report;

                //  Try DB first
                var result = _reportService.GetByUrlAsync(baseUrl).GetAwaiter().GetResult();

                if (result.Succeeded && result.Data?.LayoutData != null)
                {
                    using var layoutStream = new MemoryStream(result.Data.LayoutData);
                    report = new XtraReport();
                    report.LoadLayoutFromXml(layoutStream);
                }
                else
                {
                    report = _reportFactory.Create(baseUrl);
                }

                // Set Department Parameter - support multiple departments
                if (departmentIds.Count > 0 && report.Parameters["Department"] != null)
                {
                    var param = report.Parameters["Department"];
                    param.SelectAllValues = false;
                    param.Value = departmentIds.ToArray();
                    param.Visible = true;
                    param.Enabled = false;
                }

                using var ms = new MemoryStream();
                report.SaveLayoutToXml(ms);
                return ms.ToArray();
            }
            catch (FaultException ex)
            {
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

                urls[ReportConstants.BaseReportTemplate] = "Base Report Template";
                urls[ReportConstants.AllowanceItemsReportTemplate] = "Allowance Items Report";
                urls[ReportConstants.UserReportTemplate] = "Users Report";
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
                    throw new FaultException($"Failed to update report: {updateResult.Message}");
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
                ReportStatusId = ReportStatuses.Draft,
                Url = url,
                LayoutData = layoutData
            };

            var createResult = _reportService.CreateAsync(createDto).GetAwaiter().GetResult();

            if (!createResult.Succeeded)
                throw new FaultException(createResult.Message);

            return createResult.Data ?? url;
        }
    }
}

