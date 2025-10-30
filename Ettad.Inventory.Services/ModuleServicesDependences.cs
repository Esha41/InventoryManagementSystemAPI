using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using Ettad.Inventory.Services.Ammunitions;

namespace Ettad.Inventory.Services
{
    public static class ModuleServicesDependences
    {
        public static IServiceCollection AddInventoryServices(this IServiceCollection services)
        {
            // Register AutoMapper profiles from this assembly
            services.AddAutoMapper(Assembly.GetExecutingAssembly());

            // Register FluentValidation validators from this assembly
            services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

            // Register services
            services.AddScoped<IAmmunitionService, AmmunitionService>();

            return services;
        }
    }
}

