using Ettad.Inventory.Service.Weapons.Dtos;
using Ettad.Inventory.Service.Weapons.Validators;
using Ettad.Inventory.Service.Explosives.Dtos;
using Ettad.Inventory.Service.Explosives.Validators;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using Ettad.Inventory.Service.Ammunitions.Dtos;
using Ettad.Inventory.Service.Ammunitions.Validators;
using Ettad.Inventory.Service.Assets.Dtos;
using Ettad.Inventory.Service.Assets.Validators;
using Ettad.Inventory.Service.AssetSupply.Dtos;
using Ettad.Inventory.Service.AssetSupply.Validators;
using Ettad.Inventory.Service.Batches.Dtos;
using Ettad.Inventory.Service.Batches.Validators;
using Ettad.Inventory.Service.AllowanceItems.Interfaces;
using Ettad.Inventory.Service.Ammunitions.Interfaces;
using Ettad.Inventory.Service.AssetHistory.Interfaces;
using Ettad.Inventory.Service.AssetSupply.Interfaces;
using Ettad.Inventory.Service.Batches.Interfaces;
using Ettad.Inventory.Service.Common.Interfaces;
using Ettad.Inventory.Service.Explosives.Interfaces;
using Ettad.Inventory.Service.Inventories.Interfaces;
using Ettad.Inventory.Service.ItemDepartmentAssignments.Interfaces;
using Ettad.Inventory.Service.Monitoring.Interfaces;
using Ettad.Inventory.Service.Weapons.Interfaces;
using Ettad.Inventory.Service.AllowanceItems.Services;
using Ettad.Inventory.Service.Ammunitions.Services;
using Ettad.Inventory.Service.AssetHistory.Services;
using Ettad.Inventory.Service.AssetSupply.Services;
using Ettad.Inventory.Service.Batches.Services;
using Ettad.Inventory.Service.Explosives.Services;
using Ettad.Inventory.Service.Inventories.Services;
using Ettad.Inventory.Service.ItemDepartmentAssignments.Services;
using Ettad.Inventory.Service.Monitoring.Services;
using Ettad.Inventory.Service.Weapons.Services;
using Ettad.Inventory.Service.Weapons.Mapper;
using Ettad.Inventory.Service.Ammunitions.Mapper;
using Ettad.Inventory.Service.Explosives.Mapper;
using Ettad.Inventory.Service.ItemDepartmentAssignments.Mapper;
using Ettad.Inventory.Service.Batches.Mapper;
using Ettad.Inventory.Service.Assets.Mapper;
using Ettad.Inventory.Service.AssetSupply.Mapper;
using Ettad.Inventory.Service.AssetHistory.Mapper;
using Ettad.Inventory.Service.Assets.Implementation;
using Ettad.Inventory.Service.Assets.Background;
using Ettad.Inventory.Service.Assets.Interfaces;
using Ettad.Inventory.Service.Employees.Interfaces;
using Ettad.Inventory.Service.Employees.Implementation;
using Ettad.Inventory.Service.Monitoring.BackgroundJobs;

namespace Ettad.Inventory.Service
{
    public static class ModuleServicesDependencies
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
            services.AddScoped<IInventoryDashboardMonitoringService, InventoryDashboardMonitoringService>();
            services.AddScoped<LowStockEmailTemplateService>();
            services.AddScoped<LowStockMonitorJob>();

            // Crtitical Stock Monitor services
            services.AddScoped<ICriticalStockMonitorBackgroundService, CriticalStockMonitorBackgroundService>();
            services.AddScoped<ICriticalStockMonitorSettingsService, CriticalStockMonitorSettingsService>();
            services.AddScoped<ICriticalStockMonitoringService, CriticalStockMonitoringService>();
            services.AddScoped<CriticalStockEmailTemplateService>();
            services.AddScoped<CriticalStockMonitorJob>();
            services.AddScoped<IExcelImportService, ExcelImportService>();

            // Asset Supply Services
            services.AddScoped<IAssetSupplyService, AssetSupplyService>();
            services.AddScoped<IValidator<CreateAssetSupplyDto>, CreateAssetSupplyDtoValidator>();
            services.AddScoped<IValidator<ReturnAssetDto>, ReturnAssetDtoValidator>();
            services.AddScoped<IValidator<ReturnMultipleAssetsDto>, ReturnMultipleAssetsDtoValidator>();
            services.AddAutoMapper(typeof(AssetSupplyMappingProfile));


            services.AddScoped<IExcelExportService, ExcelExportService>();

            // Asset History Services
            services.AddScoped<IAssetBulkDeletionProcessor, AssetBulkDeletionProcessor>();
            services.AddScoped<IAssetBulkDeleteJobScheduler, HangfireAssetBulkDeleteJobScheduler>();
            services.AddScoped<Ettad.Inventory.Service.Assets.Background.AssetBulkDeleteHangfireJob>();
            services.AddScoped<IBulkAssetDeleteService, BulkAssetDeleteService>();

            services.AddScoped<IAssetHistoryService, AssetHistoryService>();
            services.AddAutoMapper(typeof(AssetHistoryMappingProfile));

            return services;
        }
    }
}

