namespace Ettad.Application.Common.Interfaces;

/// <summary>
/// Service for checking user permissions
/// </summary>
public interface IPermissionService
{
    /// <summary>
    /// Check if the current user has a specific permission
    /// </summary>
    /// <param name="permissionName">The permission name to check</param>
    /// <returns>True if user has the permission, false otherwise</returns>
    Task<bool> HasPermissionAsync(string permissionName);
    
    /// <summary>
    /// Get all permissions for a specific user
    /// </summary>
    /// <param name="userId">The user ID</param>
    /// <returns>List of permission names</returns>
    Task<List<string>> GetUserPermissions(string userId);

    /// <summary>Clears cached permission lists for this user (call after role assignment or default role change).</summary>
    Task InvalidatePermissionCacheForUserAsync(string userId);
}
