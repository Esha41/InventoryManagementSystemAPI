using DevExpress.XtraReports.Web.Extensions;
using Ettad.ReportManagement.Service.Interfaces;
using Ettad.ReportManagement.Service.Services;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Ettad.ReportManagement.Service
{
    public static class ModuleServicesDependences
    {
        public static IServiceCollection AddReportManagementServices(this IServiceCollection services)
        {
            // Register Report services
            services.AddScoped<IReportService, ReportService>();
            services.AddScoped<IScheduledReportService, ScheduledReportService>();
            services.AddScoped<IScheduledReportExecutionService, ScheduledReportExecutionService>();

            // Register custom report storage extension
            services.AddScoped<Reports.Factories.ReportFactory>();
            services.AddScoped<ReportStorageWebExtension, Reports.CustomReportStorageWebExtension>();

            services.AddAutoMapper(Assembly.GetExecutingAssembly());

            return services;
        }
    }
}
