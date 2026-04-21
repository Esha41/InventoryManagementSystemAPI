namespace Ettad.Application.Common.Interfaces;

/// <summary>
/// Resolves AspNetRole id(s) for the user's active session (default role if valid, else sole role, else none).
/// </summary>
public interface IEffectiveRoleService
{
    /// <summary>
    /// Returns zero or one role id: empty when the user is missing, has no roles, or must pick among multiple;
    /// one id when the effective session role is unambiguous.
    /// </summary>
    Task<IReadOnlyList<string>> GetEffectiveRoleIdsAsync(string userId, CancellationToken cancellationToken = default);

    /// <summary>
    /// The single effective role id, or null when ambiguous or missing (same cases as <see cref="GetEffectiveRoleIdsAsync"/>).
    /// </summary>
    Task<string?> GetEffectiveRoleIdAsync(string userId, CancellationToken cancellationToken = default);
}
