using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using Ettad.Announcement.Service.Validators;
using MediatR;
using Ettad.Announcement.Service.Interfaces;
using Ettad.Announcement.Service.Services;

namespace Ettad.Announcement.Service
{
    public static class ModuleServicesDependences
    {
        public static IServiceCollection AddAnnouncementServices(this IServiceCollection services)
        {
            // Register MediatR handlers (e.g. UserLoggedOutEventHandler)
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));

            // Register services
            services.AddScoped<IAnnouncementService, AnnouncementService>();

            // Register validators
            services.AddValidatorsFromAssemblyContaining<CreateAnnouncementDtoValidator>();

            // Register AutoMapper
            services.AddAutoMapper(typeof(ModuleServicesDependences).Assembly);

            return services;
        }
    }
}
