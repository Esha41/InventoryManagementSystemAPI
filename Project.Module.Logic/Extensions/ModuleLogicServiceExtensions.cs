using System.Reflection;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace Ettad.Module.Logic.Extensions
{
    public static class ModuleLogicServiceExtensions
    {
        public static IServiceCollection AddModuleLogicServices(this IServiceCollection services)
        {
            // Register MediatR
            services.AddMediatR(cfg => {
                cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
            });

            // Register Validators
            services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

            // Register Validation Behavior
         //   services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));

            return services;
        }
    }
} 