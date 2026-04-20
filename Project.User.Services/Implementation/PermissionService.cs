using Ettad.Application.Common.Interfaces;
using Ettad.Comman.Idenitity;
using Ettad.CrossCutting.Comman.Idenitity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;

namespace Ettad.User.Services.Implementation;

/// <summary>
/// Service for checking user permissions with caching support
/// </summary>
public class PermissionService : IPermissionService
{
    private readonly ICurrentUserService _currentUserService;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly RoleManager<ApplicationRole> _roleManager;
    private readonly IEffectiveRoleService _effectiveRoleService;
    private readonly IMemoryCache _cache;
    private readonly ILogger<PermissionService> _logger;
    private const int CacheExpirationMinutes = 5;

    public PermissionService(
        ICurrentUserService currentUserService,
        UserManager<ApplicationUser> userManager,
        RoleManager<ApplicationRole> roleManager,
        IEffectiveRoleService effectiveRoleService,
        IMemoryCache cache,
        ILogger<PermissionService> logger)
    {
        _currentUserService = currentUserService;
        _userManager = userManager;
        _roleManager = roleManager;
        _effectiveRoleService = effectiveRoleService;
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
        var effectiveRole = !string.IsNullOrEmpty(effectiveRoleId)
            ? await _roleManager.FindByIdAsync(effectiveRoleId)
            : null;
        if (effectiveRole == null)
            return new List<string>();

        var cacheKey = $"user_permissions_{userId}_{effectiveRole.Id}";
        if (_cache.TryGetValue(cacheKey, out List<string>? cachedPermissions) && cachedPermissions != null)
        {
            _logger.LogInformation("Returning cached permissions for user {UserId} role {RoleId}: {Count} permissions", userId, effectiveRole.Id, cachedPermissions.Count);
            return cachedPermissions;
        }

        _logger.LogInformation("Loading permissions from database for user {UserId} active role {RoleName}", userId, effectiveRole.Name);

        var permissions = new List<string>();

        var roleClaims = await _roleManager.GetClaimsAsync(effectiveRole);
        _logger.LogInformation("Role {RoleName} has {Count} claims", effectiveRole.Name, roleClaims.Count);

        foreach (var claim in roleClaims)
        {
            if (!string.IsNullOrWhiteSpace(claim.Type) && !permissions.Contains(claim.Type))
                permissions.Add(claim.Type);

            if (!string.IsNullOrWhiteSpace(claim.Value) && !permissions.Contains(claim.Value))
                permissions.Add(claim.Value);
        }

        _logger.LogInformation("Loaded {Count} total permissions for user {UserId}", permissions.Count, userId);

        _cache.Set(cacheKey, permissions, TimeSpan.FromMinutes(CacheExpirationMinutes));

        return permissions;
    }

    public async Task InvalidatePermissionCacheForUserAsync(string userId)
    {
        if (string.IsNullOrEmpty(userId))
            return;

        _cache.Remove($"user_permissions_{userId}");

        var user = await _userManager.FindByIdAsync(userId);
        if (user == null)
            return;

        var roleNames = await _userManager.GetRolesAsync(user);
        foreach (var name in roleNames)
        {
            var r = await _roleManager.FindByNameAsync(name);
            if (r != null)
                _cache.Remove($"user_permissions_{userId}_{r.Id}");
        }
    }
}