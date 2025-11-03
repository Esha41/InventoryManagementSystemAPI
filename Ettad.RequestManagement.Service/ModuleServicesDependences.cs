using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using FluentValidation;
using System.Reflection;
using Ettad.RequestManagement.Service.Requests;


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
            services.AddScoped<IRequestService, RequestServices>();
          

            return services;
        }
    }
}

