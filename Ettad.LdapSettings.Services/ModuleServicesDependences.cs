using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using Ettad.LdapSettings.Services.Interfaces;
using Ettad.LdapSettings.Services.Validators;
using Ettad.LdapSettings.Services.Services;
using Ettad.LdapSettings.Services.Dtos;

namespace Ettad.LdapSettings.Services
{
    public static class ModuleServicesDependences
    {
        public static IServiceCollection AddLdapSettingsServices(this IServiceCollection services)
        {
            // Register FluentValidation validators from this assembly
            services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

            // Register services
            services.AddScoped<ILdapSettingsService, LdapSettingsService>();
            services.AddScoped<IValidator<LdapOptions>, LdapOptionsValidator>();

            return services;
        }
    }
}

