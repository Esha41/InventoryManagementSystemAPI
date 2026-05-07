using Ettad.Comman.Idenitity;
using Ettad.Data.Interfaces.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Ettad.Repository.Repositories;

public class EffectiveRoleRepository : IEffectiveRoleRepository
{
    private readonly ICrossCuttingRepository<ApplicationUser> _userRepository;
    private readonly ICrossCuttingRepository<IdentityUserRole<string>> _userRoleRepository;

    public EffectiveRoleRepository(
        ICrossCuttingRepository<ApplicationUser> userRepository,
        ICrossCuttingRepository<IdentityUserRole<string>> userRoleRepository)
    {
        _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
        _userRoleRepository = userRoleRepository ?? throw new ArgumentNullException(nameof(userRoleRepository));
    }

    public async Task<IReadOnlyList<string>> GetEffectiveRoleIdsAsync(string userId, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrEmpty(userId))
            return Array.Empty<string>();

        var user = await _userRepository
            .Find(u => u.Id == userId)
            .FirstOrDefaultAsync(cancellationToken);
        if (user == null)
            return Array.Empty<string>();

        var assigned = await _userRoleRepository
            .Find(ur => ur.UserId == userId)
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
