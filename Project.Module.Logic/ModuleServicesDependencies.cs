using Ettad.Data.Entities;
using Ettad.Module.lookup.Dtos;
using Ettad.Module.lookup.Interfaces;
using Ettad.Module.lookup.Mapper;
using Ettad.Module.lookup.Services;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Ettad.Module.Logic.Extensions
{
    public static class ModuleServicesDependencies
    {
        public static IServiceCollection AddLookUpervices(this IServiceCollection services)
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

            services.AddScoped<UnitLookupService>();
            services.AddScoped<IUnitLookupService>(sp => sp.GetRequiredService<UnitLookupService>());
            services.AddScoped<ILookupService<Ettad.Data.Entities.Unit, CreateUpdateUnitDto>>(sp => sp.GetRequiredService<UnitLookupService>());

            services.AddScoped<CaliberLookupService>();
            services.AddScoped<ICaliberLookupService>(sp => sp.GetRequiredService<CaliberLookupService>());
            services.AddScoped<ILookupService<Caliber, CreateUpdateCaliberDto>>(sp => sp.GetRequiredService<CaliberLookupService>());

            // Register Validators
            services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

            // Register Mapper
            services.AddAutoMapper(typeof(LookupMappingProfile));

            return services;
        }
    }
}