using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Ettad.Comman.Idenitity;
using Ettad.Common.Interfaces;
using Ettad.CrossCutting.Comman.Idenitity;


namespace Ettad.User.Services
{
public class PermissionService : IPermissionService
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly RoleManager<ApplicationRole> _roleManager;
    public PermissionService(UserManager<ApplicationUser> userManager, RoleManager<ApplicationRole> roleManager)
    {
        _userManager = userManager;
        _roleManager = roleManager;
    }

    public async Task<List<string>> GetUserPermissions(string userId)
    {
        var roleClaims = new List<string>();

        var user = await _userManager.FindByIdAsync(userId);
        if (user == null)
            return new List<string>();

        var roles = await _userManager.GetRolesAsync(user);

        // 3. Collect claims for each role
        foreach (var roleName in roles)
        {
            var role = await _roleManager.FindByNameAsync(roleName);
            if (role != null)
            {
                var claims = await _roleManager.GetClaimsAsync(role);


                roleClaims.AddRange(claims.Select
                      (x =>
                           x.Value
                       

                      ));
            }
        }

        return roleClaims;
    }
}
}