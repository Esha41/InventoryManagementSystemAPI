using Ettad.Comman.Idenitity;
using Ettad.CrossCutting.Comman.Idenitity;
using Ettad.Data.Entities;
using Ettad.Data.Entities.Settings;
using Ettad.Data.Enums;
using Ettad.EntityFramework.DataBaseContext.DataSeeding;
using Ettad.EntityFramework.Utiliies;
using Ettad.Infrastructure.Utilities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Ettad.EntityFramework.DataBaseContext
{
    public abstract class ApplicationDbcontextSeed
    {
        public static async Task SeedDefaultUserAsync(
            ApplicationDbContext context,
            UserManager<ApplicationUser> userManager,
            RoleManager<ApplicationRole> roleManager)
        {
            try
            {
                // ========================
                // ADMINISTRATOR ROLE
                // ========================
                var administratorRole = new ApplicationRole
                {
                    Name = "Administrator",
                    NameAr = "مسؤول النظام",
                    IsSuperAdmin = true,
                    IsDefaultRole = false
                };

                var plainPermissions = PlainPermissionsGenerator.GetPlainPermissionsWithGroup();
                var crudPermissions = await new CrudPermissionsGenerator(context).GenerateAllPermissions();

                var existingAdminRole = await roleManager.FindByNameAsync(administratorRole.Name);
                if (existingAdminRole == null)
                {
                    await roleManager.CreateAsync(administratorRole);
                    existingAdminRole = await roleManager.FindByNameAsync(administratorRole.Name);
                }

                // Remove existing claims
                var existingClaims = await roleManager.GetClaimsAsync(existingAdminRole);
                foreach (var claim in existingClaims)
                    await roleManager.RemoveClaimAsync(existingAdminRole, claim);

                // Add CRUD permissions
                foreach (var crudModel in crudPermissions)
                {
                    foreach (var crudPermission in crudModel.PermissionsList)
                    {
                        if (!string.IsNullOrWhiteSpace(crudPermission.DisplayValue))
                        {
                            await roleManager.AddClaimAsync(existingAdminRole, new Claim("Permissions", crudPermission.DisplayValue));
                        }
                    }
                }

                // Add plain permissions
                foreach (var plainModel in plainPermissions)
                {
                    foreach (var plainPermission in plainModel.PermissionsList)
                    {
                        if (plainPermission.DisplayValue != "BasedOnEntity")
                            await roleManager.AddClaimAsync(existingAdminRole, new Claim("Permissions", plainPermission.DisplayValue));
                    }
                }

                // Ensure role claims are saved
                await context.SaveChangesAsync();

                // ========================
                // ADMINISTRATOR USER
                // ========================
                var administrator = new ApplicationUser
                {
                    IsLdapUser = false,
                    IsSuperAdmin = true,
                    EmailConfirmed = true,
                    Email = "administrator@localhost",
                    UserName = "administrator@localhost",
                    FullNameEN = "Super Admin",
                    FullNameAR = "مدير النظام"
                };

                if (userManager.Users.All(u => u.UserName != administrator.UserName))
                {
                    await userManager.CreateAsync(administrator, "Administrator1!");
                    await userManager.AddToRolesAsync(administrator, new[] { administratorRole.Name });

                    // Refresh security stamp to ensure claims are loaded on first login
                    await userManager.UpdateSecurityStampAsync(administrator);
                }

                // ========================
                // SEED APPLICATION ENTITIES AND ROLES
                // ========================
                await SeedApplicationEntitiesAndRolesAsync(context, roleManager);
            }
            catch (Exception ex)
            {
                // log the exception
                Console.WriteLine($"Seeding error: {ex.Message}");
                throw;
            }
        }

        private static async Task SeedApplicationEntitiesAndRolesAsync(ApplicationDbContext context, RoleManager<ApplicationRole> roleManager)
        {
            // Ensure ApplicationEntities are seeded
            await SeedApplicationEntitiesAsync(context);

            // Get all ApplicationEntities
            var entities = await context.ApplicationEntities
                .Where(e => !e.IsDeleted)
                .ToListAsync();

            // Define roles for each entity based on the image
            var entityRoles = new Dictionary<string, List<(string NameEn, string NameAr)>>
            {
                {
                    "Order Requesting Entity", new List<(string, string)>
                    {
                        ("Order Requester", "مقدم الطلب"),
                        ("Supply Officer", "ضابط الامداد"),
                        ("Requesting Entity Commander", "قائد القوات")
                    }
                },
                {
                    "Directorate of Armament", new List<(string, string)>
                    {
                        ("Auditor of Ammunition Division", "مدقق شعبة الذخيرة"),
                        ("Head of Ammunition Division", "رئيس شعبة الذخيرة"),
                        ("Director of the Armament Entity", "مدير مديرية التسليح"),
                        ("Head of the Armament Entity", "رئيس الهيئة")
                    }
                },
                {
                    "Military Operation", new List<(string, string)>
                    {
                        ("Officer", "ضابط هيئة العمليات"),
                        ("Auditor", "مدقق هيئة العمليات"),
                        ("Chief of Operations", "رئيس هيئة العمليات")
                    }
                },
                {
                    "Chief of Staff", new List<(string, string)>
                    {
                        ("Auditor", "مدقق"),
                        ("Deputy Chief of Staff for Operations", "نائب رئيس الأركان للعمليات المشتركة"),
                        ("Chief of Staff", "رئيس الأركان")
                    }
                },
                {
                    "Military Training", new List<(string, string)>
                    {
                        ("Military Training Auditor", "مدقق التدريب العسكري"),
                        ("Military Training Officer", "ضابط التدريب العسكري"),
                        ("Head of Military Training", "رئيس التدريب العسكري")
                    }
                },
                {
                    "Inventory", new List<(string, string)>
                    {
                        ("Auditor of Audit Depo", "مدقق شعبة المراقبة"),
                        ("Head of Audit Depo", "رئيس شعبة المراقبة"),
                        ("Depo Division Auditor", "مدقق شعبة المستودعات"),
                        ("Head of Depo Division", "رئيس شعبة المستودعات"),
                        ("Depo Commander", "قائد المستودعات"),
                        ("Depo Officer", "ضابط المستودع")
                    }
                }
            };

            var utcNow = DateTime.UtcNow;
            var roleApplicationEntities = new List<RoleApplicationEntity>();

            // Create roles for each entity
            foreach (var entity in entities)
            {
                if (entityRoles.ContainsKey(entity.NameEn))
                {
                    var rolesForEntity = entityRoles[entity.NameEn];

                    foreach (var (roleNameEn, roleNameAr) in rolesForEntity)
                    {
                        // Format role name with entity name: "Role Name (Entity Name)"
                        var fullRoleNameEn = $"{roleNameEn} ({entity.NameEn})";
                        var fullRoleNameAr = $"{roleNameAr} ({entity.NameAr})";

                        // Check if role already exists
                        var existingRole = await roleManager.FindByNameAsync(fullRoleNameEn);
                        if (existingRole == null)
                        {
                            // Create new role
                            var role = new ApplicationRole
                            {
                                Name = fullRoleNameEn,
                                NameAr = fullRoleNameAr,
                                IsSuperAdmin = false,
                                IsDefaultRole = false
                            };

                            var result = await roleManager.CreateAsync(role);
                            if (result.Succeeded)
                            {
                                existingRole = await roleManager.FindByNameAsync(fullRoleNameEn);
                            }
                        }

                        if (existingRole != null)
                        {
                            // Assign permissions to role based on PermissionConfig
                            await AssignPermissionsToRoleAsync(roleManager, existingRole, roleNameEn, entity.NameEn);

                            // Check if role-entity link already exists
                            var existingLink = await context.RoleApplicationEntities
                                .FirstOrDefaultAsync(r => r.RoleId == existingRole.Id && r.ApplicationEntityId == entity.Id);

                            if (existingLink == null)
                            {
                                roleApplicationEntities.Add(new RoleApplicationEntity
                                {
                                    RoleId = existingRole.Id,
                                    ApplicationEntityId = entity.Id,
                                    CreationDate = utcNow,
                                    CreatedBy = "SYSTEM"
                                });
                            }
                        }
                    }
                }
            }

            // Add all role-entity links
            if (roleApplicationEntities.Any())
            {
                await context.RoleApplicationEntities.AddRangeAsync(roleApplicationEntities);
                await context.SaveChangesAsync();
            }
        }

        private static async Task AssignPermissionsToRoleAsync(
            RoleManager<ApplicationRole> roleManager,
            ApplicationRole role,
            string roleNameEn,
            string entityNameEn)
        {
            // Get permissions for this role from PermissionConfig
            List<string> permissions = GetPermissionsForRole(roleNameEn, entityNameEn);

            if (permissions == null || !permissions.Any())
            {
                return; // No permissions configured for this role
            }

            // Remove existing claims
            var existingClaims = await roleManager.GetClaimsAsync(role);
            foreach (var claim in existingClaims)
            {
                await roleManager.RemoveClaimAsync(role, claim);
            }

            // Add permissions as claims
            foreach (var permission in permissions)
            {
                await roleManager.AddClaimAsync(role, new Claim("Permissions", permission));
            }
        }

        private static List<string> GetPermissionsForRole(string roleNameEn, string entityNameEn)
        {
            // Map role names and entity names to PermissionConfig properties
            var permissionMap = new Dictionary<(string RoleName, string EntityName), Func<List<string>>>
            {
                // Order Requesting Entity
                { ("Order Requester", "Order Requesting Entity"), () => PermissionConfig.Requester_OrderRequestingEntity },
                { ("Supply Officer", "Order Requesting Entity"), () => PermissionConfig.SupplyOfficer_OrderRequestingEntity },
                { ("Requesting Entity Commander", "Order Requesting Entity"), () => PermissionConfig.RequestingEntityCommander_OrderRequestingEntity },

                // Military Training Entity
                { ("Military Training Officer", "Military Training"), () => PermissionConfig.MilitaryTrainingOfficer_MilitaryTrainingEntity },
                { ("Military Training Auditor", "Military Training"), () => PermissionConfig.MilitaryTrainingAuditor_MilitaryTrainingEntity },
                { ("Head of Military Training", "Military Training"), () => PermissionConfig.HeadOfMiltaryTraining_MilitaryTrainingEntity },

                // Directorate of Armament Entity
                { ("Auditor of Ammunition Division", "Directorate of Armament"), () => PermissionConfig.Auditor_DirectorateOfAmmunitionEntity },
                { ("Head of Ammunition Division", "Directorate of Armament"), () => PermissionConfig.HeadOfDivision_DirectorateOfAmmunitionEntity },
                { ("Director of the Armament Entity", "Directorate of Armament"), () => PermissionConfig.DirectorOfArmament_DirectorateOfAmmunitionEntity },
                { ("Head of the Armament Entity", "Directorate of Armament"), () => PermissionConfig.HeadOfLogistics_DirectorateOfAmmunitionEntity },

                // Military Operations Entity
                { ("Officer", "Military Operation"), () => PermissionConfig.Officer_MilitaryOperationsEntity },
                { ("Auditor", "Military Operation"), () => PermissionConfig.Auditor_MilitaryOperationsEntity },
                { ("Chief of Operations", "Military Operation"), () => PermissionConfig.HeadOfMilitaryOperations_MilitaryOperationsEntity },

                // Chief of Staff Entity
                { ("Auditor", "Chief of Staff"), () => PermissionConfig.Auditor_ChiefOfStaffEntity },
                { ("Deputy Chief of Staff for Operations", "Chief of Staff"), () => PermissionConfig.DeputyChiefOfStaff_ChiefOfStaffEntity },
                { ("Chief of Staff", "Chief of Staff"), () => PermissionConfig.ChiefOfStaff_ChiefOfStaffEntity },

                // Inventory Entity
                { ("Auditor of Audit Depo", "Inventory"), () => PermissionConfig.AuditorOfAuditDepo_InventoryEntity },
                { ("Head of Audit Depo", "Inventory"), () => PermissionConfig.HeadOfAuditDepo_InventoryEntity },
                { ("Depo Division Auditor", "Inventory"), () => PermissionConfig.DepoDivisionAuditor_InventoryEntity },
                { ("Head of Depo Division", "Inventory"), () => PermissionConfig.DepoCommander_InventoryEntity },
                { ("Depo Commander", "Inventory"), () => PermissionConfig.DepoCommander_InventoryEntity },
                { ("Depo Officer", "Inventory"), () => PermissionConfig.DepoOfficer_InventoryEntity }
            };

            var key = (roleNameEn, entityNameEn);
            if (permissionMap.TryGetValue(key, out var permissionGetter))
            {
                return permissionGetter();
            }

            return null;
        }

        private static async Task SeedApplicationEntitiesAsync(ApplicationDbContext context)
        {
            try
            {
                // Check if ApplicationEntities already exist
                if (await context.ApplicationEntities.AnyAsync())
                {
                    return; // Already seeded via configuration
                }

                var utcNow = DateTime.UtcNow;

                // Seed ApplicationEntities based on the image
                var applicationEntities = new[]
                {
                    new ApplicationEntity
                    {
                        Code = "ORE",
                        NameAr = "القوة الطالبة",
                        NameEn = "Order Requesting Entity",
                        IsDeleted = false,
                        CreationDate = utcNow,
                        CreatedBy = "SYSTEM"
                    },
                    new ApplicationEntity
                    {
                        Code = "MT",
                        NameAr = "مديرية التدريب العسكري",
                        NameEn = "Military Training",
                        IsDeleted = false,
                        CreationDate = utcNow,
                        CreatedBy = "SYSTEM"
                    },
                    new ApplicationEntity
                    {
                        Code = "DoA",
                        NameAr = "مديرية التسليح",
                        NameEn = "Directorate of Armament",
                        IsDeleted = false,
                        CreationDate = utcNow,
                        CreatedBy = "SYSTEM"
                    },
                    new ApplicationEntity
                    {
                        Code = "MO",
                        NameAr = "العمليات",
                        NameEn = "Military Operation",
                        IsDeleted = false,
                        CreationDate = utcNow,
                        CreatedBy = "SYSTEM"
                    },
                    new ApplicationEntity
                    {
                        Code = "CoS",
                        NameAr = "مكتب رئيس الأركان",
                        NameEn = "Chief of Staff",
                        IsDeleted = false,
                        CreationDate = utcNow,
                        CreatedBy = "SYSTEM"
                    },
                    new ApplicationEntity
                    {
                        Code = "Inventory",
                        NameAr = "مستودعات الأسلحة والذخيرة المركزيه",
                        NameEn = "Inventory",
                        IsDeleted = false,
                        CreationDate = utcNow,
                        CreatedBy = "SYSTEM"
                    }
                };

                await context.ApplicationEntities.AddRangeAsync(applicationEntities);
                await context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                // log the exception
                Console.WriteLine($"Seeding ApplicationEntities error: {ex.Message}");
                throw;
            }
        }
    }
}
