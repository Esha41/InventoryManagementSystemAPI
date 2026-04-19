using Ettad.Comman.Idenitity;
using Ettad.EntityFramework.DataBaseContext;
using Microsoft.EntityFrameworkCore;

namespace Ettad.EntityFramework.Helpers;

/// <summary>
/// Resolves AspNetRole ids that represent the user's active session (DefaultRoleId or sole role).
/// Matches <see cref="Ettad.User.Services.Helpers.EffectiveRoleResolver"/> rules.
/// </summary>
public static class EffectiveAspNetRoleIds
{
    public static async Task<List<string>> ForUserAsync(ApplicationDbContext context, string userId, CancellationToken cancellationToken = default)
    {
        var user = await context.Users.AsNoTracking().FirstOrDefaultAsync(u => u.Id == userId, cancellationToken);
        if (user == null)
            return new List<string>();

        var assigned = await context.UserRoles
            .AsNoTracking()
            .Where(ur => ur.UserId == userId)
            .Select(ur => ur.RoleId)
            .ToListAsync(cancellationToken);

        if (assigned.Count == 0)
            return new List<string>();

        if (!string.IsNullOrEmpty(user.DefaultRoleId) && assigned.Contains(user.DefaultRoleId))
            return new List<string> { user.DefaultRoleId };

        if (assigned.Count == 1)
            return new List<string> { assigned[0] };

        return new List<string>();
    }
}
