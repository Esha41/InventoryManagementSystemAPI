using Microsoft.Extensions.DependencyInjection;
using Ettad.Services.Helpers;
using Ettad.Services.Mapper;
using System.Reflection;

namespace Ettad.Workflow.Service
{
    public static class ModuleServicesDependences
    {
        public static IServiceCollection AddWorkflowServices(this IServiceCollection service)
        {
            service.AddAutoMapper(typeof(MappingProfile));
            service.AddTransient<IHelpureService, HelpureService>();

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