using Ettad.Comman.Idenitity;
using Ettad.CrossCutting.Comman.Idenitity;
using Microsoft.AspNetCore.Identity;

namespace Ettad.User.Services.Helpers;

/// <summary>Resolves the single active <see cref="ApplicationRole"/> for permission and JWT claims.</summary>
public static class EffectiveRoleResolver
{
    /// <summary>
    /// Returns the effective role: <see cref="ApplicationUser.DefaultRoleId"/> if valid;
    /// otherwise the sole assigned role when exactly one exists; otherwise null (multi-role, must select).
    /// </summary>
    public static async Task<ApplicationRole?> ResolveAsync(
        UserManager<ApplicationUser> userManager,
        RoleManager<ApplicationRole> roleManager,
        ApplicationUser user)
    {
        var roleNames = await userManager.GetRolesAsync(user);
        if (roleNames.Count == 0)
            return null;

        var roles = new List<ApplicationRole>();
        foreach (var name in roleNames)
        {
            var r = await roleManager.FindByNameAsync(name);
            if (r != null)
                roles.Add(r);
        }

        if (roles.Count == 0)
            return null;

        if (!string.IsNullOrEmpty(user.DefaultRoleId))
        {
            var match = roles.FirstOrDefault(x => x.Id == user.DefaultRoleId);
            if (match != null)
                return match;
        }

        if (roles.Count == 1)
            return roles[0];

        return null;
    }
}
