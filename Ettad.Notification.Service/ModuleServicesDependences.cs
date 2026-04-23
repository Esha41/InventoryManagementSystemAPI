using FluentValidation;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using Ettad.Notification.Service.Hubs;
using Ettad.Notification.Service.EmailTemplate;
using MediatR;
using Ettad.Notification.Service.Interfaces;
using Ettad.Notification.Service.Services;

namespace Ettad.Notification.Service
{
    public static class ModuleServicesDependences
    {
        public static IServiceCollection AddNotificationServices(this IServiceCollection services)
        {
            // Register AutoMapper profiles from this assembly
            services.AddAutoMapper(Assembly.GetExecutingAssembly());

            // Register FluentValidation validators from this assembly
            services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

            // Register MediatR handlers from this assembly
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));

            // Register SignalR
            services.AddSignalR();

            // Register services
            services.AddScoped<INotificationService, NotificationService>();
            services.AddScoped<INotificationHelperService, NotificationHelperService>();
            services.AddScoped<IEmailTemplateService, EmailTemplateService>();

            return services;
        }
    }
}
