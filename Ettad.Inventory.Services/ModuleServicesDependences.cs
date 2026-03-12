using Ettad.Inventory.Service.Weapons;
using Ettad.Inventory.Service.Weapons.Dtos;
using Ettad.Inventory.Service.Weapons.Validators;
using Ettad.Inventory.Service.Weapons.Profiles;
using Ettad.Inventory.Service.Explosives;
using Ettad.Inventory.Service.Explosives.Dtos;
using Ettad.Inventory.Service.Explosives.Validators;
using Ettad.Inventory.Service.Explosives.Profiles;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using Ettad.Inventory.Service.Ammunitions;
using Ettad.Inventory.Service.Ammunitions.Dtos;
using Ettad.Inventory.Service.Ammunitions.Validators;
using Ettad.Inventory.Service.Ammunitions.Profiles;
using Ettad.Inventory.Service.AllowanceItems;
using Ettad.Inventory.Service.Inventories;
using Ettad.Inventory.Service.Monitoring;
using Ettad.Inventory.Services.Common;
using Ettad.Inventory.Service.Assets;
using Ettad.Inventory.Service.Assets.Dtos;
using Ettad.Inventory.Service.Assets.Validators;
using Ettad.Inventory.Service.Assets.Profiles;
using Ettad.Inventory.Service.AssetSupply;
using Ettad.Inventory.Service.AssetSupply.Dtos;
using Ettad.Inventory.Service.AssetSupply.Validators;
using Ettad.Inventory.Service.AssetSupply.Profiles;
using Ettad.Inventory.Service.AssetHistory;
using Ettad.Inventory.Service.AssetHistory.Profiles;
using Ettad.Inventory.Service.ItemDepartmentAssignments;
using Ettad.Inventory.Service.ItemDepartmentAssignments.Profiles;
using Ettad.Inventory.Service.Batches;
using Ettad.Inventory.Service.Batches.Dtos;
using Ettad.Inventory.Service.Batches.Validators;
using Ettad.Inventory.Service.Batches.Profiles;

namespace Ettad.Inventory.Service
{
    public static class ModuleServicesDependences
    {
        public static IServiceCollection AddInventoryServices(this IServiceCollection services)
        {
            // Register AutoMapper profiles from this assembly
            // Register FluentValidation validators from this assembly
            services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

            // Register services
            // Ammunition Services
            services.AddScoped<IAmmunitionService, AmmunitionService>();
            services.AddScoped<IValidator<CreateUpdateAmmunitionDto>, CreateUpdateAmmunitionDtoValidator>();
            services.AddAutoMapper(typeof(AmmunitionMappingProfile));

            // Weapon Services
            services.AddScoped<IWeaponService, WeaponService>();
            services.AddScoped<IValidator<CreateUpdateWeaponDto>, CreateUpdateWeaponDtoValidator>();
            services.AddAutoMapper(typeof(WeaponMappingProfile));

            // Explosive Services
            services.AddScoped<IExplosiveService, ExplosiveService>();
            services.AddScoped<IValidator<CreateUpdateExplosiveDto>, CreateUpdateExplosiveDtoValidator>();
            services.AddAutoMapper(typeof(ExplosiveMappingProfile));

            services.AddScoped<IAllowanceItemService, AllowanceItemService>();
            services.AddScoped<IAllowanceItemQueryService, AllowanceItemQueryService>();
            services.AddScoped<IItemDepartmentAssignmentService, ItemDepartmentAssignmentService>();
            services.AddAutoMapper(typeof(ItemDepartmentAssignmentMappingProfile));
            services.AddScoped<IInventoryService, InventoryService>();
            
            // Batch Services (must be registered before AssetService since AssetService depends on IBatchService)
            services.AddScoped<IBatchService, BatchService>();
            services.AddScoped<IValidator<BulkUpdateBatchAssetsDto>, BulkUpdateBatchAssetsDtoValidator>();
            services.AddAutoMapper(typeof(BatchMappingProfile));

            // Asset Services
            services.AddScoped<IAssetService, AssetService>();
            services.AddScoped<IValidator<CreateAssetDto>, CreateAssetDtoValidator>();
            services.AddScoped<IValidator<UpdateAssetDto>, UpdateAssetDtoValidator>();
            services.AddAutoMapper(typeof(AssetMappingProfile));

            // Employee Services
            services.AddScoped<IEmployeeService, EmployeeService>();
            services.AddScoped<IValidator<CreateUpdateEmployeeDto>, CreateUpdateEmployeeDtoValidator>();
            services.AddAutoMapper(typeof(EmployeeMappingProfile));
            
            // Low Stock Monitor services
            services.AddScoped<ILowStockMonitorBackgroundService, LowStockMonitorBackgroundService>();
            services.AddScoped<ILowStockMonitorSettingsService, LowStockMonitorSettingsService>();
            services.AddScoped<ILowStockMonitoringService, LowStockMonitoringService>();
            services.AddScoped<IExpiringLotMonitoringService, ExpiringLotMonitoringService>();
            services.AddScoped<LowStockEmailTemplateService>();
            services.AddScoped<LowStockMonitorJob>();
            services.AddScoped<IExcelImportService, ExcelImportService>();

            // Asset Supply Services
            services.AddScoped<IAssetSupplyService, AssetSupplyService>();
            services.AddScoped<IValidator<CreateAssetSupplyDto>, CreateAssetSupplyDtoValidator>();
            services.AddScoped<IValidator<ReturnAssetDto>, ReturnAssetDtoValidator>();
            services.AddScoped<IValidator<ReturnMultipleAssetsDto>, ReturnMultipleAssetsDtoValidator>();
            services.AddAutoMapper(typeof(AssetSupplyMappingProfile));

            // Asset History Services
            services.AddScoped<IAssetHistoryService, AssetHistoryService>();
            services.AddAutoMapper(typeof(AssetHistoryMappingProfile));

            return services;
        }
    }
}
