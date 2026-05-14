using Ettad.Services.Helpers;
using Ettad.Services.Mapper;
using Ettad.Workflow.Service.Interface;
using Ettad.Workflows.Service.Monitoring;
using Ettad.Workflows.Service.Services;
using Ettad.Workflows.Service.Settings;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Ettad.Workflow.Service
{
    public static class ModuleServicesDependencies
    {
        public static IServiceCollection AddWorkflowServices(this IServiceCollection service)
        {
            service.AddAutoMapper(typeof(Ettad.Services.Mapper.MappingProfile), typeof(Ettad.Workflows.Service.Mapper.MappingProfile));
            service.AddTransient<IHelpureService, HelpureService>();
            
            // Register WorkflowStepNotifierService
            service.AddScoped<IWorkflowStepNotifierService, WorkflowStepNotifierService>();
            service.AddScoped<IWorkflowStartNotificationService, WorkflowStartNotificationService>();
            service.AddScoped<IOrderAutoRejectBackgroundService, OrderAutoRejectBackgroundService>();
            service.AddScoped<IOrderAutoRejectSettingsService, OrderAutoRejectSettingsService>();
            service.AddScoped<RequestAutoRejectCountdownService>();
            service.AddScoped<IRequestAutoRejectCountdownService>(sp => sp.GetRequiredService<RequestAutoRejectCountdownService>());
            service.AddScoped<IOrderAutoRejectCountdownService>(sp => sp.GetRequiredService<RequestAutoRejectCountdownService>());
            service.AddSingleton<IWorkflowAutoRejectConfigCache, WorkflowAutoRejectConfigCache>();
            service.AddScoped<OrderAutoRejectHangfireJob>();

            // Register MediatR from multiple assemblies
            service.AddMediatR(cfg =>
            {
                cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()); // Current assembly
                cfg.RegisterServicesFromAssembly(typeof(Ettad.Workflows.Service.Queries.GetWorkflowById.GetWorkflowByIdQueryHandler).Assembly); // Workflows assembly
            });

            return service;
        }
    }
}