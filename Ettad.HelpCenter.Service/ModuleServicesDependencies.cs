using Ettad.HelpCenter.Service.Interfaces;
using Ettad.HelpCenter.Service.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Ettad.HelpCenter.Service
{
    public static class ModuleServicesDependencies
    {
        public static IServiceCollection AddHelpCenterServices(this IServiceCollection services)
        {
            services.AddScoped<IHelpCenterService, HelpCenterService>();
            return services;
        }
    }
}
