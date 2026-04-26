using Ettad.Module.lookup.Interfaces;
using Ettad.Module.lookup.Services;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Win32;
using System.Reflection;

namespace Ettad.Module.Logic.Extensions
{
    public static class ModuleServicesDependences
    {
        public static IServiceCollection AddModuleLogicServices(this IServiceCollection services)
        {
            // Register MediatR
            services.AddMediatR(cfg => {
                cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
            });

            // Register services
            services.AddScoped<IDepotService, DepotService>();
            services.AddScoped<IDepotAccessService, DepotAccessService>();
            services.AddScoped<IUserDepotService, UserDepotService>();
            services.AddScoped(typeof(ILookupService<,>), typeof(LookupService<,>));

            // Register Validators
            services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

            return services;
        }
    }
}