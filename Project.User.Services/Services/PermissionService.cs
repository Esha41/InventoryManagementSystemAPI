using Ettad.Application.Common.Interfaces;
using Ettad.Comman.Idenitity;
using Ettad.CrossCutting.Comman.Idenitity;
using Ettad.CrossCutting.Comman.Time;
using Ettad.Data.Entities;
using Ettad.Data.Interfaces.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ettad.User.Services.Services;

/// <summary>
/// Service for checking user permissions with caching support
/// </summary>
public class PermissionService : IPermissionService
{
    private readonly ICurrentUserService _currentUserService;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly RoleManager<ApplicationRole> _roleManager;
    private readonly IEffectiveRoleRepository _effectiveRoleService;
    private readonly ICrossCuttingRepository<UserDelegation> _userDelegationRepository;
    private readonly IDateTimeProvider _dateTimeProvider;
    private readonly IMemoryCache _cache;
    private readonly ILogger<PermissionService> _logger;
    private const int CacheExpirationMinutes = 5;

    public PermissionService(
        ICurrentUserService currentUserService,
        UserManager<ApplicationUser> userManager,
        RoleManager<ApplicationRole> roleManager,
        IEffectiveRoleRepository effectiveRoleService,
        ICrossCuttingRepository<UserDelegation> userDelegationRepository,
        IDateTimeProvider dateTimeProvider,
        IMemoryCache cache,
        ILogger<PermissionService> logger)
    {
        _currentUserService = currentUserService;
        _userManager = userManager;
        _roleManager = roleManager;
        _effectiveRoleService = effectiveRoleService;
        _userDelegationRepository = userDelegationRepository;
        _dateTimeProvider = dateTimeProvider;
        _cache = cache;
        _logger = logger;
    }

    public async Task<bool> HasPermissionAsync(string permissionName)
    {
        if (string.IsNullOrWhiteSpace(permissionName))
            return false;

        // Get all user permissions (cached)
        var userId = _currentUserService.UserId;
        if (string.IsNullOrEmpty(userId))
            return false;

        var permissions = await GetUserPermissions(userId);
        
        _logger.LogInformation("Checking permission {Permission} for user {UserId}. User has {Count} permissions: {Permissions}", 
            permissionName, userId, permissions.Count, string.Join(", ", permissions));
        
        // Check if user has the permission
        var hasPermission = permissions.Contains(permissionName, StringComparer.OrdinalIgnoreCase);
        
        _logger.LogInformation("Permission check result for {Permission}: {HasPermission}", permissionName, hasPermission);
        
        return hasPermission;
    }

    public async Task<List<string>> GetUserPermissions(string userId)
    {
        if (string.IsNullOrEmpty(userId))
            return new List<string>();

        var user = await _userManager.Users.FirstOrDefaultAsync(u => u.Id == userId);
        if (user == null)
        {
            _logger.LogWarning("User {UserId} not found", userId);
            return new List<string>();
        }

        var effectiveRoleId = await _effectiveRoleService.GetEffectiveRoleIdAsync(userId);

        // Roles inherited via active delegations: the delegatee gains exactly the role each
        // delegator was logged in with (captured at creation), for as long as the delegation
        // is active (date-bounded, so expiry is lazy and automatic).
        var delegatedRoleIds = await GetActiveDelegatedRoleIdsAsync(userId);

        if (string.IsNullOrEmpty(effectiveRoleId) && delegatedRoleIds.Count == 0)
            return new List<string>();

        // The cache key embeds the effective role plus the (sorted) delegated role set, so any
        // approve/revoke/expiry that changes the active delegation set yields a different key
        // and a fresh computation automatically.
        var effectivePart = string.IsNullOrEmpty(effectiveRoleId) ? "none" : effectiveRoleId;
        var delegPart = delegatedRoleIds.Count == 0
            ? "none"
            : string.Join("-", delegatedRoleIds);
        var version = GetPermissionCacheVersion(userId);
        var cacheKey = $"user_permissions_{userId}_v{version}_{effectivePart}_{delegPart}";

        if (_cache.TryGetValue(cacheKey, out List<string> cachedPermissions) && cachedPermissions != null)
        {
            _logger.LogInformation("Returning cached permissions for user {UserId} key {CacheKey}: {Count} permissions", userId, cacheKey, cachedPermissions.Count);
            return cachedPermissions;
        }

        _logger.LogInformation("Loading permissions from database for user {UserId} (effective role {RoleId}, {DelegCount} delegated roles)", userId, effectivePart, delegatedRoleIds.Count);

        var permissions = new List<string>();

        var effectiveRole = !string.IsNullOrEmpty(effectiveRoleId)
            ? await _roleManager.FindByIdAsync(effectiveRoleId)
            : null;
        if (effectiveRole != null)
            await AddRoleClaimsAsync(effectiveRole, permissions);

        foreach (var delegatedRoleId in delegatedRoleIds)
        {
            var delegatedRole = await _roleManager.FindByIdAsync(delegatedRoleId);
            if (delegatedRole != null)
                await AddRoleClaimsAsync(delegatedRole, permissions);
        }

        _logger.LogInformation("Loaded {Count} total permissions for user {UserId}", permissions.Count, userId);

        _cache.Set(cacheKey, permissions, TimeSpan.FromMinutes(CacheExpirationMinutes));

        return permissions;
    }

    private async Task AddRoleClaimsAsync(ApplicationRole role, List<string> permissions)
    {
        var roleClaims = await _roleManager.GetClaimsAsync(role);
        foreach (var claim in roleClaims)
        {
            if (!string.IsNullOrWhiteSpace(claim.Type) && !permissions.Contains(claim.Type))
                permissions.Add(claim.Type);

            if (!string.IsNullOrWhiteSpace(claim.Value) && !permissions.Contains(claim.Value))
                permissions.Add(claim.Value);
        }
    }

    /// <summary>
    /// Returns the distinct, sorted role ids the user inherits via currently active delegations
    /// (approved, within the date window). The repository is queried directly here to avoid a
    /// dependency cycle with IUserDelegationService (which depends on IPermissionService).
    /// </summary>
    private async Task<List<string>> GetActiveDelegatedRoleIdsAsync(string delegateeUserId)
    {
        var now = _dateTimeProvider.Now;

        var roleIds = await _userDelegationRepository
            .Find(d => d.DelegateeUserId == delegateeUserId &&
                        d.IsActive && !d.IsDeleted &&
                        d.DelegationStatus == 1 &&
                        d.StartDate <= now &&
                        d.EndDate >= now &&
                        d.DelegatorRoleId != null)
            .Select(d => d.DelegatorRoleId)
            .Distinct()
            .ToListAsync();

        roleIds.Sort(StringComparer.Ordinal);
        return roleIds;
    }

    private long GetPermissionCacheVersion(string userId)
    {
        return _cache.GetOrCreate($"user_permissions_version_{userId}", entry =>
        {
            entry.Priority = CacheItemPriority.NeverRemove;
            return 0L;
        });
    }

    public Task InvalidatePermissionCacheForUserAsync(string userId)
    {
        if (string.IsNullOrEmpty(userId))
            return Task.CompletedTask;

        // Bump the per-user version so every previously cached key (which embeds the version,
        // effective role, and delegated role set) is abandoned and recomputed on next access.
        var versionKey = $"user_permissions_version_{userId}";
        var current = _cache.GetOrCreate(versionKey, entry =>
        {
            entry.Priority = CacheItemPriority.NeverRemove;
            return 0L;
        });
        _cache.Set(versionKey, current + 1, new MemoryCacheEntryOptions { Priority = CacheItemPriority.NeverRemove });

        // Clear the legacy key from the previous cache scheme for safety.
        _cache.Remove($"user_permissions_{userId}");
        return Task.CompletedTask;
    }
}