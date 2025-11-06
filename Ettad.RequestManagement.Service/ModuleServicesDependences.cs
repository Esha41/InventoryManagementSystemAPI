using Microsoft.Extensions.DependencyInjection;
using FluentValidation;
using System.Reflection;
using Ettad.RequestManagement.Service.Discards;
using Ettad.RequestManagement.Service.Common;
using Ettad.RequestManagement.Service.RequestPurposes;
using Ettad.RequestManagement.Service.Returns;

namespace Ettad.RequestManagement.Service
{
    public static class ModuleServicesDependences
    {
        public static IServiceCollection AddRequestServices(this IServiceCollection services)
        {
            // Register AutoMapper profiles from this assembly
            services.AddAutoMapper(Assembly.GetExecutingAssembly());

            // Register FluentValidation validators from this assembly
            services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

            // Register services
            services.AddScoped<IRequestNoGeneratorService, RequestNoGeneratorService>();
            services.AddScoped<IRequestPurposeService, RequestPurposeService>();
            services.AddScoped<IDiscardService, DiscardService>();
            services.AddScoped<IReturnService, ReturnService>();

            return services;
        }
    }
}

