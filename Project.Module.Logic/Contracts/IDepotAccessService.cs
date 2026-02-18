namespace Ettad.Lookups.Services.Contracts
{
    /// <summary>
    /// Service for checking if a user has access to a specific depot.
    /// </summary>
    public interface IDepotAccessService
    {
        /// <summary>
        /// Checks if the user has access to the specified depot.
        /// SuperAdmin users always have access. Other users must have an entry in UserDepots.
        /// </summary>
        /// <param name="userId">The user ID to check</param>
        /// <param name="depotId">The depot ID to check</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>True if user has access, false otherwise</returns>
        Task<bool> HasDepotAccessAsync(string userId, long depotId, CancellationToken cancellationToken = default);
    }
}
