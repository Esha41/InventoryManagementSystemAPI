using Ettad.Data.Interfaces.Repositories;
using Ettad.EntityFramework.DataBaseContext.DataSeeding;
using Ettad.Repository.Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Ettad.Repository
{
    public static class ModuleInfrastructureDependencies
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection service, IConfiguration configuration)
        {
            ApplicationDbInitializer.EnsureLogDatabaseExists(configuration);

            service.AddScoped<ITransactionManager, EfTransactionManager>();

            service.AddTransient<UnitOfWork>();

            service.AddScoped(typeof(CrossCuttingRepository<>));

            service.AddScoped(typeof(ICrossCuttingRepository<>), typeof(CrossCuttingRepository<>));

            service.AddScoped<IEffectiveRoleRepository, EffectiveRoleRepository>();

            return service;
        }
    }
}
