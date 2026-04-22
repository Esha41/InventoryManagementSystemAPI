using Ettad.Data.Interfaces.Repositories;
using Ettad.EntityFramework.DataBaseContext;
using Microsoft.EntityFrameworkCore;

namespace Ettad.Repository.Repositories;

public class EffectiveRoleRepository : IEffectiveRoleRepository
{
    private readonly ApplicationDbContext _context;

    public EffectiveRoleRepository(ApplicationDbContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    public async Task<IReadOnlyList<string>> GetEffectiveRoleIdsAsync(string userId, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrEmpty(userId))
            return Array.Empty<string>();

        var user = await _context.Users.AsNoTracking().FirstOrDefaultAsync(u => u.Id == userId, cancellationToken);
        if (user == null)
            return Array.Empty<string>();

        var assigned = await _context.UserRoles
            .AsNoTracking()
            .Where(ur => ur.UserId == userId)
            .Select(ur => ur.RoleId)
            .ToListAsync(cancellationToken);

        if (assigned.Count == 0)
            return Array.Empty<string>();

        if (!string.IsNullOrEmpty(user.DefaultRoleId) && assigned.Contains(user.DefaultRoleId))
            return new[] { user.DefaultRoleId };

        if (assigned.Count == 1)
            return new[] { assigned[0] };

        return Array.Empty<string>();
    }

    public async Task<string?> GetEffectiveRoleIdAsync(string userId, CancellationToken cancellationToken = default)
    {
        var ids = await GetEffectiveRoleIdsAsync(userId, cancellationToken);
        return ids.Count == 1 ? ids[0] : null;
    }
}
