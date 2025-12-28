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
            services.AddScoped<IInventoryService, InventoryService>();
            
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
            services.AddScoped<LowStockEmailTemplateService>();
            services.AddScoped<IExcelImportService, ExcelImportService>();

            return services;
        }
    }
}
