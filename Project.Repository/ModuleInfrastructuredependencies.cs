using Ettad.Data.Interfaces.Repositories;
using Ettad.Data.Interfaces.Services;
using Ettad.Repository.Repositories;
using Ettad.Repository.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Ettad.Repository
{
    public static class ModuleInfrastructureDependencies
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection service, IConfiguration configuration)
        {
            service.AddScoped<ITransactionManager, EfTransactionManager>();

            service.AddTransient<UnitOfWork>();

            service.AddScoped(typeof(CrossCuttingRepository<>));

            service.AddScoped(typeof(ICrossCuttingRepository<>), typeof(CrossCuttingRepository<>));

            service.AddScoped<IEffectiveRoleRepository, EffectiveRoleRepository>();

            service.AddScoped<IAssetBulkSqlRepository, AssetBulkSqlRepository>();

            service.AddScoped<IAssetBulkDeletionPersistence, AssetBulkDeletionPersistence>();

            service.AddScoped<IInventoryPermanentDeleteExecutor, InventoryPermanentDeleteExecutor>();

            service.AddScoped<ISystemAttachmentSlotResolver, SystemAttachmentSlotResolver>();

            return service;
        }
    }
}
