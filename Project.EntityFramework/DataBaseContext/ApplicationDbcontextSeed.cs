using Microsoft.AspNetCore.Identity;
using Newtonsoft.Json;
using Ettad.Comman.Idenitity;
using Ettad.CrossCutting.Comman.Idenitity;
using Ettad.Data.Entities.Settings;
using Ettad.Data.Enums;
using Ettad.EntityFramework.Utiliies;
using Ettad.Infrastructure.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

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
                ApplicationRole administratorRole = new()
                {
                    IsDefaultRole = false,
                    IsSuperAdmin = true,
                    Name = "Administrator",
                };
                var plainPermissions = PlainPermissionsGenerator.GetPlainPermissionsWithGroup();
                var crudPermissions = await new CrudPermissionsGenerator(context).GenerateAllPermissions();
                // Check if the role already exists
                var existingRole = await roleManager.FindByNameAsync(administratorRole.Name);

                if (existingRole == null)
                {
                    // Create the role if it doesn't exist
                    await roleManager.CreateAsync(administratorRole);
                    existingRole = await roleManager.FindByNameAsync(administratorRole.Name);
                }

                // Remove all existing claims for the role
                var existingClaims = await roleManager.GetClaimsAsync(existingRole);
                foreach (var claim in existingClaims)
                {
                    await roleManager.RemoveClaimAsync(existingRole, claim);
                }

                // Add CRUD permissions
                foreach (var crudModel in crudPermissions)
                {
                    foreach (var crudPermission in crudModel.PermissionsList)
                    {
                        if (!string.IsNullOrWhiteSpace(crudPermission.DisplayValue))
                        {
                            await roleManager.AddClaimAsync(existingRole, new Claim("Permissions", crudPermission.DisplayValue));
                        }
                    }
                }

                // Add plain permissions
                foreach (var plainModel in plainPermissions)
                {
                    foreach (var plainPermission in plainModel.PermissionsList)
                    {
                        if (plainPermission.DisplayValue != "BasedOnEntity")
                        {
                            await roleManager.AddClaimAsync(existingRole, new Claim("Permissions", plainPermission.DisplayValue));
                        }
                    }
                }

                var administrator = new ApplicationUser
                {
                    IsLdapUser = false,
                    IsSuperAdmin = true,
                    EmailConfirmed = true,
                    Email = "administrator@localhost",
                    UserName = "administrator@localhost",
                    FullNameEN= "Super Admin",
                    FullNameAR="مدير النظام",
                };
                if (userManager.Users.All(item => item.UserName != administrator.UserName))
                {
                    await userManager.CreateAsync(administrator, "Administrator1!");
                    await userManager.AddToRolesAsync(administrator, new[] { administratorRole.Name });
                }
            }
            catch(Exception ex)
            {

            }
        }

        public static async Task SeedEmailConfigurationAsync(ApplicationDbContext context)
        {
            try
            {
                await context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                // log exception (avoid empty catch)
                throw;
            }
        }
    }
}
