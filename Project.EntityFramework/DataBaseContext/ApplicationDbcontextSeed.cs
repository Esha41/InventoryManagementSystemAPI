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
                // ORDER REQUESTOR ROLE
                // ========================
                var requestorRole = await roleManager.FindByNameAsync("Order Requestor");
                if (requestorRole == null)
                {
                    requestorRole = new ApplicationRole
                    {
                        Name = "Order Requestor",
                        IsSuperAdmin = false,
                        IsDefaultRole = false
                    };
                    await roleManager.CreateAsync(requestorRole);
                }

                // Remove existing claims
                existingClaims = await roleManager.GetClaimsAsync(requestorRole);
                foreach (var claim in existingClaims)
                    await roleManager.RemoveClaimAsync(requestorRole, claim);

                // Add requestor permissions
                foreach (var permission in PermissionConfig.Requester)
                {
                    await roleManager.AddClaimAsync(requestorRole, new Claim("Permissions", permission));
                }

                await context.SaveChangesAsync();

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

        private static async Task SeedApplicationEntitiesAsync(ApplicationDbContext context)
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
                    Id = 1,
                    Code = "ORE",
                    NameAr = "القوة الطالبة",
                    NameEn = "Order Requesting Entity",
                    IsDeleted = false,
                    CreationDate = utcNow,
                    CreatedBy = "SYSTEM"
                },
                new ApplicationEntity
                {
                    Id = 2,
                    Code = "MT",
                    NameAr = "مديرية التدريب العسكري",
                    NameEn = "Military Training",
                    IsDeleted = false,
                    CreationDate = utcNow,
                    CreatedBy = "SYSTEM"
                },
                new ApplicationEntity
                {
                    Id = 3,
                    Code = "DoA",
                    NameAr = "مديرية التسليح",
                    NameEn = "Directorate of Armament",
                    IsDeleted = false,
                    CreationDate = utcNow,
                    CreatedBy = "SYSTEM"
                },
                new ApplicationEntity
                {
                    Id = 4,
                    Code = "MO",
                    NameAr = "العمليات",
                    NameEn = "Military Operation",
                    IsDeleted = false,
                    CreationDate = utcNow,
                    CreatedBy = "SYSTEM"
                },
                new ApplicationEntity
                {
                    Id = 5,
                    Code = "CoS",
                    NameAr = "مكتب رئيس الأركان",
                    NameEn = "Chief of Staff",
                    IsDeleted = false,
                    CreationDate = utcNow,
                    CreatedBy = "SYSTEM"
                },
                new ApplicationEntity
                {
                    Id = 6,
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
    }
}
