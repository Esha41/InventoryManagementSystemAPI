using Microsoft.Extensions.DependencyInjection;
using FluentValidation;
using System.Reflection;
using Ettad.RequestManagement.Service.Discards;
using Ettad.RequestManagement.Service.Common;
using Ettad.RequestManagement.Service.RequestPurposes;
using Ettad.RequestManagement.Service.Returns;
using Ettad.RequestManagement.Service.Orders;
using Ettad.RequestManagement.Service.SupplyManagement;
using Ettad.RequestManagement.Service.OrderItemTracking;
using Ettad.RequestManagement.Service.Implementation;
using Ettad.RequestManagement.Service.Interfaces;
using Ettad.Data.Interfaces.Services;

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

            // Register MediatR handlers
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));

            // Register services
            services.AddScoped<IRequestNoGeneratorService, RequestNoGeneratorService>();
            services.AddScoped<IRequestPurposeService, RequestPurposeService>();
            services.AddScoped<IDiscardService, DiscardService>();
            services.AddScoped<IReturnService, ReturnService>();
            services.AddScoped<IOrderService, OrderService>();
            services.AddScoped<ISupplyService, SupplyService>();
            services.AddScoped<IWorkflowSupplySummaryService, WorkflowSupplySummaryService>();
            services.AddScoped<IRequestService, RequestService>();
            services.AddScoped<IOrderItemTrackingService, OrderItemTrackingService>();

            return services;
        }
    }
}
