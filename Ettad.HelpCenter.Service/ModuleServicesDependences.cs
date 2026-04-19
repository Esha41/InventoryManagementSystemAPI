using Microsoft.Extensions.DependencyInjection;

namespace Ettad.HelpCenter.Service
{
    public static class ModuleServicesDependences
    {
        public static IServiceCollection AddHelpCenterServices(this IServiceCollection services)
        {
            services.AddScoped<IHelpCenterService, HelpCenterService>();
            return services;
        }
    }
}
