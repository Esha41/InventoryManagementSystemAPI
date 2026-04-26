using Ettad.Data.Interfaces.Repositories;
using Ettad.Repository.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace Ettad.Repository
{
    public static class ModuleInfrastructureDependences
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection service)
        {
            service.AddScoped<ITransactionManager, EfTransactionManager>();

            service.AddTransient<UnitOfWork>();

            service.AddScoped(typeof(CrossCuttingRepository<>));

            service.AddScoped(typeof(ICrossCuttingRepository<>), typeof(CrossCuttingRepository<>));

            return service;
        }
    }
}
