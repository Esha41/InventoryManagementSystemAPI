using DevExpress.XtraReports.Web.Extensions;
using Ettad.ReportManagement.Service.Interfaces;
using Ettad.ReportManagement.Service.Mapper;
using Ettad.ReportManagement.Service.Services;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Ettad.ReportManagement.Service
{
    public static class ModuleServicesDependencies
    {
        public static IServiceCollection AddReportManagementServices(this IServiceCollection services)
        {
            // Register Report services
            services.AddScoped<IReportService, ReportService>();
            services.AddScoped<IScheduledReportService, ScheduledReportService>();
            services.AddScoped<IScheduledReportExecutionService, ScheduledReportExecutionService>();
            services.AddScoped<Reports.Factories.ReportFactory>();
            services.AddScoped<ReportStorageWebExtension, Reports.CustomReportStorageWebExtension>();
            services.AddAutoMapper(Assembly.GetExecutingAssembly());

            //Register Mapper
            services.AddAutoMapper(typeof(ReportManagementMappingProfile));

            // Register IServiceProvider and IServiceScope as trusted types to allow ObjectDataSource deserialization
            DevExpress.Utils.DeserializationSettings.RegisterTrustedClass(typeof(IServiceProvider));
            DevExpress.Utils.DeserializationSettings.RegisterTrustedClass(typeof(IServiceScope));

            return services;
        }
    }
}
