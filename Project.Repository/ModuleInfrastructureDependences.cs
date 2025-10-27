using Microsoft.Extensions.DependencyInjection;
using Ettad.Repository.Repository;

namespace Ettad.Repository
{
    public static class ModuleInfrastructureDependences
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection service)
        {
            service.AddTransient<UnitOfWork>();
            return service;
        }
    }
}
