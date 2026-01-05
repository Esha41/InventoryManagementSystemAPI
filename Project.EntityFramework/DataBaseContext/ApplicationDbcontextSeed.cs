using Ettad.Comman.Idenitity;
using Ettad.CrossCutting.Comman.Idenitity;
using Ettad.Data.Entities;
using Ettad.EntityFramework.DataBaseContext.DataSeeding;
using Ettad.EntityFramework.DataBaseContext.DataSeeding.Workflows;
using Ettad.EntityFramework.Utiliies;
using Ettad.Infrastructure.Utilities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text.RegularExpressions;

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
                
                // ========================
                // SEED DEFAULT USERS FOR ALL ROLES
                // ========================
                await SeedDefaultUsersForRolesAsync(context, userManager, roleManager);
                
                await SeedWorkflows.SeedNormalOrderWorkflowAsync(context);
                await SeedWorkflows.SeedNoramlOrderForTrainingPurposeWorkflowAsync(context);
                await SeedWorkflows.SeedOrderFromAllowanceWorkflowAsync(context);
                await SeedWorkflows.SeedNormalOrder_Weapon_WorkflowAsync(context);
                await SeedWorkflows.SeedNoramlOrderForTrainingPurpose_Weapon_WorkflowAsync(context);
                await SeedWorkflows.SeedOrderFromAllowance_Weapon_WorkflowAsync(context);
                await SeedWorkflows.SeedDiscardWorkflowAsync(context);
                await SeedWorkflows.SeedReturnWorkflowAsync(context);
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
                        ("Supply Officer", "ضابط الإمداد"),
                        ("Requesting Entity Commander", "قائد القوات")
                    }
                },
                {
                    "Directorate of Armament", new List<(string, string)>
                    {
                        ("Auditor of Ammunition Division", "مدقق شعبة الذخيرة"),
                        ("Head of Ammunition Division", "رئيس شعبة الذخيرة"),
                        ("Auditor of Weapons Division", "مدقق شعبة السلاح"),
                        ("Head of Weapons Division", "رئيس شعبة السلاح"),
                        ("Director of the Armament Entity", "مدير مديرية التسليح"),
                        ("Head of Logistics", "رئيس الهيئة")
                    }
                },
                {
                    "Military Operation", new List<(string, string)>
                    {
                        ("Auditor of Military Operation", "مدقق هيئة العمليات"),
                        ("Officer of Military Operation", "ضابط هيئة العمليات"),
                        ("Chief of Operations", "رئيس هيئة العمليات")
                    }
                },
                {
                    "Chief of Staff Office", new List<(string, string)>
                    {
                        ("Auditor of Chief of Staff Office", "مدقق مكتب رئيس الأركان"),
                        ("Auditor of Deputy of Chief of Staff Office", "مدقق مكتب نائب رئيس الأركان"),
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
                        ("Depo Commander", "قائد المستودعات"),
                        ("Auditor of Depo Division", "مدقق شعبة المستودعات"),
                        ("Head of Depo Division", "رئيس شعبة المستودعات"),
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
                { ("Military Training Auditor", "Military Training"), () => PermissionConfig.MilitaryTrainingAuditor_MilitaryTrainingEntity },
                { ("Military Training Officer", "Military Training"), () => PermissionConfig.MilitaryTrainingOfficer_MilitaryTrainingEntity },
                { ("Head of Military Training", "Military Training"), () => PermissionConfig.HeadOfMiltaryTraining_MilitaryTrainingEntity },

                // Directorate of Armament Entity
                { ("Auditor of Ammunition Division", "Directorate of Armament"), () => PermissionConfig.AuditorOfAmmunitionDivision_DirectorateOfAmmunitionEntity },
                { ("Head of Ammunition Division", "Directorate of Armament"), () => PermissionConfig.HeadOfAmmunitionDivision_DirectorateOfAmmunitionEntity },
                { ("Auditor of Weapons Division", "Directorate of Armament"), () => PermissionConfig.AuditorOfWeaponsDivision_DirectorateOfAmmunitionEntity },
                { ("Head of Weapons Division", "Directorate of Armament"), () => PermissionConfig.HeadOfWeaponsDivision_DirectorateOfAmmunitionEntity },
                { ("Director of the Armament Entity", "Directorate of Armament"), () => PermissionConfig.DirectorOfArmament_DirectorateOfAmmunitionEntity },
                { ("Head of Logistics", "Directorate of Armament"), () => PermissionConfig.HeadOfLogistics_DirectorateOfAmmunitionEntity },

                // Military Operations Entity
                { ("Auditor of Military Operation", "Military Operation"), () => PermissionConfig.Auditor_MilitaryOperationsEntity },
                { ("Officer of Military Operation", "Military Operation"), () => PermissionConfig.Officer_MilitaryOperationsEntity },
                { ("Chief of Operations", "Military Operation"), () => PermissionConfig.HeadOfMilitaryOperations_MilitaryOperationsEntity },

                // Chief of Staff Entity
                { ("Auditor of Chief of Staff Office", "Chief of Staff Office"), () => PermissionConfig.Auditor_ChiefOfStaffOfficeEntity },
                { ("Auditor of Deputy of Chief of Staff Office", "Chief of Staff Office"), () => PermissionConfig.AuditorOfDeputy_ChiefOfStaffOfficeEntity },
                { ("Deputy Chief of Staff for Operations", "Chief of Staff Office"), () => PermissionConfig.DeputyChiefOfStaff_ChiefOfStaffOfficeEntity },
                { ("Chief of Staff", "Chief of Staff Office"), () => PermissionConfig.ChiefOfStaff_ChiefOfStaffOfficeEntity },

                // Inventory Entity
                { ("Auditor of Audit Depo", "Inventory"), () => PermissionConfig.AuditorOfAuditDepo_InventoryEntity },
                { ("Head of Audit Depo", "Inventory"), () => PermissionConfig.HeadOfAuditDepo_InventoryEntity },
                { ("Depo Commander", "Inventory"), () => PermissionConfig.DepoCommander_InventoryEntity },
                { ("Auditor of Depo Division", "Inventory"), () => PermissionConfig.AuditorOfDepoDivision_InventoryEntity },
                { ("Head of Depo Division", "Inventory"), () => PermissionConfig.HeadOfDepoDivision_InventoryEntity },
                { ("Depo Officer", "Inventory"), () => PermissionConfig.DepoOfficer_InventoryEntity }
            };

            var key = (roleNameEn, entityNameEn);
            if (permissionMap.TryGetValue(key, out var permissionGetter))
            {
                return permissionGetter();
            }

            return null;
        }

        private static readonly DepartmentRoleTemplate[] DepartmentOrderRoleTemplates =
        {
            new(
                "Order Requester (Order Requesting Entity)",
                "requester",
                "Requester",
                "مقدم الطلب"),
            new(
                "Supply Officer (Order Requesting Entity)",
                "supply.officer",
                "Supply Officer",
                "ضابط الإمداد"),
            new(
                "Requesting Entity Commander (Order Requesting Entity)",
                "commander",
                "Requesting Entity Commander",
                "قائد القوات")
        };

        private static async Task SeedDefaultUsersForRolesAsync(
            ApplicationDbContext context,
            UserManager<ApplicationUser> userManager,
            RoleManager<ApplicationRole> roleManager)
        {
            try
            {
                var departmentRoleNames = new HashSet<string>(DepartmentOrderRoleTemplates.Select(t => t.RoleName));

                // Get all roles except Administrator (which already has a user) and department-specific requester roles
                var allRoles = await roleManager.Roles
                    .Where(r => r.Name != "Administrator" && !departmentRoleNames.Contains(r.Name) && !r.IsSuperAdmin)
                    .ToListAsync();

                const string defaultPassword = "Password1!";

                foreach (var role in allRoles)
                {
                    // Generate username from role name
                    // Example: "Order Requester (Order Requesting Entity)" -> "order.requester.order.requesting.entity@localhost"
                    var username = GenerateUsernameFromRoleName(role.Name);
                    var email = username;

                    // Check if user already exists
                    var existingUser = await userManager.FindByNameAsync(username);
                    if (existingUser != null)
                    {
                        continue; // User already exists, skip
                    }

                    // Extract role name and entity name for display names
                    var (roleNameEn, entityNameEn) = ParseRoleName(role.Name);

                    // Create new user
                    var user = new ApplicationUser
                    {
                        IsLdapUser = false,
                        IsSuperAdmin = false,
                        EmailConfirmed = true,
                        Email = email,
                        UserName = username,
                        FullNameEN = roleNameEn ?? role.Name,
                        FullNameAR = role.NameAr ?? roleNameEn ?? role.Name
                    };

                    var createResult = await userManager.CreateAsync(user, defaultPassword);
                    if (createResult.Succeeded)
                    {
                        // Assign user to role
                        var addToRoleResult = await userManager.AddToRolesAsync(user, new[] { role.Name });
                        if (!addToRoleResult.Succeeded)
                        {
                            Console.WriteLine($"Failed to add user {username} to role {role.Name}: {string.Join(", ", addToRoleResult.Errors.Select(e => e.Description))}");
                        }
                        else
                        {
                            // Refresh security stamp to ensure claims are loaded on first login
                            await userManager.UpdateSecurityStampAsync(user);
                        }
                    }
                    else
                    {
                        Console.WriteLine($"Failed to create user {username}: {string.Join(", ", createResult.Errors.Select(e => e.Description))}");
                    }
                }

                await SeedDepartmentRoleUsersAsync(context, userManager, roleManager, defaultPassword);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Seeding default users error: {ex.Message}");
                throw;
            }
        }

        private static async Task SeedDepartmentRoleUsersAsync(
            ApplicationDbContext context,
            UserManager<ApplicationUser> userManager,
            RoleManager<ApplicationRole> roleManager,
            string defaultPassword)
        {
            var departments = await context.Departments
                .Where(d => !d.IsDeleted)
                .ToListAsync();

            if (!departments.Any())
            {
                return;
            }

            var roleLookup = new Dictionary<string, ApplicationRole>();
            foreach (var template in DepartmentOrderRoleTemplates)
            {
                var role = await roleManager.FindByNameAsync(template.RoleName);
                if (role != null)
                {
                    roleLookup[template.RoleName] = role;
                }
            }

            foreach (var department in departments)
            {
                foreach (var template in DepartmentOrderRoleTemplates)
                {
                    if (!roleLookup.TryGetValue(template.RoleName, out var role))
                    {
                        continue;
                    }

                    var username = GenerateDepartmentUsername(template.UsernamePrefix, department);
                    var existingUser = await userManager.FindByNameAsync(username);
                    if (existingUser != null)
                    {
                        continue;
                    }

                    var user = new ApplicationUser
                    {
                        IsLdapUser = false,
                        IsSuperAdmin = false,
                        EmailConfirmed = true,
                        Email = username,
                        UserName = username,
                        FullNameEN = $"{template.DisplayNameEn} ({department.NameEn})",
                        FullNameAR = $"{template.DisplayNameAr} ({department.NameAr})",
                        DepartmentId = department.Id
                    };

                    var createResult = await userManager.CreateAsync(user, defaultPassword);
                    if (createResult.Succeeded)
                    {
                        var addToRoleResult = await userManager.AddToRolesAsync(user, new[] { role.Name });
                        if (!addToRoleResult.Succeeded)
                        {
                            Console.WriteLine($"Failed to add user {username} to role {role.Name}: {string.Join(", ", addToRoleResult.Errors.Select(e => e.Description))}");
                        }
                        else
                        {
                            await userManager.UpdateSecurityStampAsync(user);
                        }
                    }
                    else
                    {
                        Console.WriteLine($"Failed to create user {username}: {string.Join(", ", createResult.Errors.Select(e => e.Description))}");
                    }
                }
            }
        }

        private static string GenerateUsernameFromRoleName(string roleName)
        {
            // Remove parentheses and their contents, then clean up
            var cleaned = Regex.Replace(roleName, @"\s*\([^)]*\)", "");
            
            // Convert to lowercase and replace spaces/special chars with dots
            var username = cleaned
                .ToLowerInvariant()
                .Replace(" ", ".")
                .Replace("-", ".")
                .Replace("_", ".");
            
            // Remove multiple consecutive dots
            username = Regex.Replace(username, @"\.+", ".");
            
            // Remove leading/trailing dots
            username = username.Trim('.');
            
            return $"{username}@localhost";
        }

        private static string GenerateDepartmentUsername(string prefix, Department department)
        {
            var normalizedPrefix = NormalizeForUsername(prefix);
            var normalizedDept = NormalizeForUsername(department.Code ?? department.NameEn);
            var username = $"{normalizedPrefix}.{normalizedDept}";
            username = Regex.Replace(username, @"\.+", ".");
            username = username.Trim('.');
            return $"{username}@localhost";
        }

        private static string NormalizeForUsername(string value)
        {
            var normalized = value.ToLowerInvariant();
            normalized = Regex.Replace(normalized, @"[^a-z0-9]+", ".");
            normalized = normalized.Trim('.');

            if (string.IsNullOrWhiteSpace(normalized))
            {
                normalized = "user";
            }

            return normalized;
        }

        private static (string? RoleNameEn, string? EntityNameEn) ParseRoleName(string fullRoleName)
        {
            // Parse "Role Name (Entity Name)" format
            var match = Regex.Match(fullRoleName, @"^(.+?)\s*\((.+?)\)$");
            
            if (match.Success)
            {
                var roleName = match.Groups[1].Value.Trim();
                var entityName = match.Groups[2].Value.Trim();
                return (roleName, entityName);
            }
            
            return (fullRoleName, null);
        }

        private record DepartmentRoleTemplate(
            string RoleName,
            string UsernamePrefix,
            string DisplayNameEn,
            string DisplayNameAr);

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
                        NameAr = "هئية العمليات",
                        NameEn = "Military Operation",
                        IsDeleted = false,
                        CreationDate = utcNow,
                        CreatedBy = "SYSTEM"
                    },
                    new ApplicationEntity
                    {
                        Code = "CoS",
                        NameAr = "مكتب رئيس الأركان",
                        NameEn = "Chief of Staff Office",
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
