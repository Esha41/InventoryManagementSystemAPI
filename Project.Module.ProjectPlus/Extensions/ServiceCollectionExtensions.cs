using System.Reflection;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Project.Module.ProjectPlus.Behaviors;

namespace Project.Module.ProjectPlus.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddProjectPlusModule(this IServiceCollection services)
        {
            // Register MediatR
            services.AddMediatR(cfg => {
                cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
            });

            // Register Validators
            services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

            // Register Validation Behavior
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));

            return services;
        }
    }
} 