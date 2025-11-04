using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using Ettad.Inventory.Service.Ammunitions;
using Ettad.Inventory.Service.AllowanceItems;
using Ettad.Inventory.Service.Inventories;

namespace Ettad.Inventory.Service
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
            services.AddScoped<IAllowanceItemService, AllowanceItemService>();
            services.AddScoped<IInventoryService, InventoryService>();

            return services;
        }
    }
}

