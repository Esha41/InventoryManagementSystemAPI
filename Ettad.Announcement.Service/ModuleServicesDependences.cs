using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Ettad.Announcement.Service.Validators;

namespace Ettad.Announcement.Service
{
    public static class ModuleServicesDependences
    {
        public static IServiceCollection AddAnnouncementServices(this IServiceCollection services)
        {
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
