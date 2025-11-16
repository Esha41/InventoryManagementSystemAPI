using Ettad.Comman.Idenitity;
using Ettad.CrossCutting.Comman.Idenitity;
using Ettad.Data.Entities.Settings;
using Ettad.Data.Enums;
using Ettad.EntityFramework.DataBaseContext.DataSeeding;
using Ettad.EntityFramework.Utiliies;
using Ettad.Infrastructure.Utilities;
using Microsoft.AspNetCore.Identity;
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
                // REQUESTOR ROLE
                // ========================
                var requestorRole = await roleManager.FindByNameAsync("Requestor");
                if (requestorRole == null)
                {
                    requestorRole = new ApplicationRole
                    {
                        Name = "Requestor",
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
                foreach (var permission in RequestorPermissionConfig.AllowedPermissions)
                {
                    await roleManager.AddClaimAsync(requestorRole, new Claim("Permissions", permission));
                }

                await context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                // log the exception
                Console.WriteLine($"Seeding error: {ex.Message}");
                throw;
            }
        }
    }
}
