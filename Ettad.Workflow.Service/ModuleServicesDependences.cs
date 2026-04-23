using Microsoft.Extensions.DependencyInjection;
using Ettad.Services.Helpers;
using Ettad.Services.Mapper;
using Ettad.Workflow.Service.Interface;
using Ettad.Workflows.Service.Monitoring;
using Ettad.Workflows.Service.Settings;
using System.Reflection;
using Ettad.Workflows.Service.Services;

namespace Ettad.Workflow.Service
{
    public static class ModuleServicesDependences
    {
        public static IServiceCollection AddWorkflowServices(this IServiceCollection service)
        {
            service.AddAutoMapper(typeof(MappingProfile));
            service.AddTransient<IHelpureService, HelpureService>();
            
            // Register WorkflowStepNotifierService
            service.AddScoped<IWorkflowStepNotifierService, WorkflowStepNotifierService>();

            service.AddScoped<IOrderAutoRejectBackgroundService, OrderAutoRejectBackgroundService>();
            service.AddScoped<IOrderAutoRejectSettingsService, OrderAutoRejectSettingsService>();
            service.AddScoped<IOrderAutoRejectCountdownService, OrderAutoRejectCountdownService>();
            service.AddScoped<OrderAutoRejectHangfireJob>();

            // Register MediatR from multiple assemblies
            service.AddMediatR(cfg =>
            {
                cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()); // Current assembly
                cfg.RegisterServicesFromAssembly(typeof(Ettad.Workflows.Service.Queries.GetWorkflowById.GetWorkflowByIdQueryHandler).Assembly); // Workflows assembly
            });

            service.AddAutoMapper(Assembly.GetExecutingAssembly());

            return service;
        }
    }
}