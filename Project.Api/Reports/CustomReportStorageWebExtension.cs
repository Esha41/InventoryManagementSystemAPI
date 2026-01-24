using DevExpress.AspNetCore.Reporting.QueryBuilder;
using DevExpress.AspNetCore.Reporting.ReportDesigner;
using DevExpress.AspNetCore.Reporting.WebDocumentViewer;
using DevExpress.XtraReports.UI;
using DevExpress.XtraReports.Web.Extensions;
using Ettad.CrossCutting.Comman.Time;
using Ettad.Data.Entities.Reports;
using Ettad.EntityFramework.DataBaseContext;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using Ettad.Application.Common.Interfaces;

namespace Ettad.Reporting.Storage
{
    /// <summary>
    /// Custom report storage extension that saves reports to the database
    /// </summary>
    public class CustomReportStorageWebExtension : ReportStorageWebExtension
    {
        private readonly ApplicationDbContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IWebHostEnvironment _hostingEnvironment;
        private readonly IDateTimeProvider _dateTimeProvider;
        private readonly IPermissionService _permissionService;
        private readonly ICurrentUserService _currentUserService;
        public CustomReportStorageWebExtension(
            ApplicationDbContext context,
            IHttpContextAccessor httpContextAccessor,
            IWebHostEnvironment hostingEnvironment,
            IDateTimeProvider dateTimeProvider,
            ICurrentUserService currentUserService,
            IPermissionService permissionService)
        {
            _context = context;
            _currentUserService = currentUserService;
            _httpContextAccessor = httpContextAccessor;
            _hostingEnvironment = hostingEnvironment;
            _dateTimeProvider = dateTimeProvider;
            _permissionService = permissionService;
        }

        public override bool CanSetData(string url)
        {

            // Check if user is super admin - super admins bypass all permission checks
            var isSuperAdmin = _currentUserService.IsSuperAdmin;
            if (isSuperAdmin)
                return true;

            // Check if user has report create or edit permissions
            var userPermissions = _permissionService?.GetUserPermissions(_currentUserService.UserId).Result;
            if (userPermissions == null)
                return false;

            return userPermissions.Contains("Permissions.Report.Create") ||
                   userPermissions.Contains("Permissions.Report.Edit");
        }

        public override bool IsValidUrl(string url)
        {
            // Validate report URL format
            return !string.IsNullOrEmpty(url) && 
                   !url.Contains("..") && 
                   url.Length < 500; // Reasonable length limit
        }

        public override byte[] GetData(string url)
        {
            try
            {
                var isSuperAdmin = _currentUserService.IsSuperAdmin;

                // Check permissions if not super admin
                if (!isSuperAdmin)
                {
                    var userPermissions = _permissionService?.GetUserPermissions(_currentUserService.UserId).Result;
                    if (userPermissions == null || 
                        (!userPermissions.Contains("Permissions.Report.View") && 
                         !userPermissions.Contains("Permissions.Report.Page")))
                    {
                        // User doesn't have permission to view reports
                        return Array.Empty<byte>();
                    }
                }

                // Try to get report from database
                var report = _context.Reports
                    .FirstOrDefault(r => r.Url == url && !r.IsDeleted);

                if (report != null)
                {
                    return report.LayoutData ?? Array.Empty<byte>();
                }

                // If not found in database, check if it's a base template
                if (url == "BaseReportTemplate")
                {
                    var baseReport = new Ettad.Reporting.Reports.BaseReportTemplate();
                    using (var ms = new MemoryStream())
                    {
                        baseReport.SaveLayoutToXml(ms);
                        return ms.ToArray();
                    }
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
                var user = _httpContextAccessor.HttpContext?.User;
                if (user == null || !user.Identity?.IsAuthenticated == true)
                    return new Dictionary<string, string>();

                var userId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (string.IsNullOrEmpty(userId))
                    return new Dictionary<string, string>();

                // Check if user is super admin - super admins bypass all permission checks
                var isSuperAdmin = user.FindFirst("IsSuperAdmin")?.Value == "true";
                
                // Check permissions if not super admin
                if (!isSuperAdmin)
                {
                    var userPermissions = _permissionService?.GetUserPermissions(userId).Result;
                    if (userPermissions == null || 
                        (!userPermissions.Contains("Permissions.Report.View") && 
                         !userPermissions.Contains("Permissions.Report.Page")))
                    {
                        // User doesn't have permission to view reports - return empty list
                        return new Dictionary<string, string>();
                    }
                }

                var reports = _context.Reports
                    .Where(r => !r.IsDeleted)
                    .OrderBy(r => r.ReportName)
                    .ToList();

                var urls = new Dictionary<string, string>();
                foreach (var report in reports)
                {
                    urls[report.Url] = report.ReportName;
                }

                // Add base template only if user has create permission
                if (isSuperAdmin || 
                    (_permissionService?.GetUserPermissions(userId).Result?.Contains("Permissions.Report.Create") == true))
                {
                    urls["BaseReportTemplate"] = "Base Report Template";
                }

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
            try
            {
                var user = _httpContextAccessor.HttpContext?.User;
                if (user == null || !user.Identity?.IsAuthenticated == true)
                    throw new UnauthorizedAccessException("User is not authenticated");

                var userId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (string.IsNullOrEmpty(userId))
                    throw new UnauthorizedAccessException("User ID not found");

                // Save report layout to memory stream
                byte[] layoutData;
                using (var ms = new MemoryStream())
                {
                    report.SaveLayoutToXml(ms);
                    layoutData = ms.ToArray();
                }

                // Check if report exists
                var existingReport = _context.Reports
                    .FirstOrDefault(r => r.Url == url && !r.IsDeleted);

                if (existingReport != null)
                {
                    // Update existing report
                    existingReport.LayoutData = layoutData;
                    existingReport.ModificationDate = _dateTimeProvider.Now;
                    existingReport.ModifiedBy = userId;
                    existingReport.ReportName = report.DisplayName ?? url;
                }
                else
                {
                    // Create new report (default to Draft status - ID = 1)
                    var newReport = new ReportEntity
                    {
                        Id = Guid.NewGuid(),
                        Url = url,
                        ReportName = report.DisplayName ?? url,
                        ReportStatusId = 1, // Default to Draft
                        LayoutData = layoutData,
                        CreationDate = _dateTimeProvider.Now,
                        CreatedBy = userId ?? string.Empty,
                        IsDeleted = false
                    };
                    _context.Reports.Add(newReport);
                }

                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error saving report: {ex.Message}");
                throw;
            }
        }

        public override string SetNewData(XtraReport report, string defaultUrl)
        {
            try
            {
                var user = _httpContextAccessor.HttpContext?.User;
                if (user == null || !user.Identity?.IsAuthenticated == true)
                    throw new UnauthorizedAccessException("User is not authenticated");

                var userId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (string.IsNullOrEmpty(userId))
                    throw new UnauthorizedAccessException("User ID not found");

                // Generate unique URL if default URL already exists
                var url = defaultUrl;
                var counter = 1;
                while (_context.Reports.Any(r => r.Url == url && !r.IsDeleted))
                {
                    url = $"{defaultUrl}_{counter}";
                    counter++;
                }

                // Save report layout
                byte[] layoutData;
                using (var ms = new MemoryStream())
                {
                    report.SaveLayoutToXml(ms);
                    layoutData = ms.ToArray();
                }

                var newReport = new ReportEntity
                {
                    Id = Guid.NewGuid(),
                    Url = url,
                    ReportName = report.DisplayName ?? url,
                    ReportStatusId = 1, // Default to Draft
                    LayoutData = layoutData,
                    CreationDate = _dateTimeProvider.Now,
                    CreatedBy = userId ?? string.Empty,
                    IsDeleted = false
                };

                _context.Reports.Add(newReport);
                _context.SaveChanges();

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
