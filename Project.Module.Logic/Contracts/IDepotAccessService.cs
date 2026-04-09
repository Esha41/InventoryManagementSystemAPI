namespace Ettad.Lookups.Services.Contracts
{
    /// <summary>
    /// Service for checking if a user has access to a specific depot.
    /// </summary>
    public interface IDepotAccessService
    {
        /// <summary>
        /// Checks if the user has access to the specified depot for read/data operations.
        /// SuperAdmin: always. Depots.ViewAll: any depot.
        /// Otherwise Depots.View or Inventory.View with a UserDepot row for that depot. Depots.Page alone does not grant access.
        /// </summary>
        /// <param name="userId">The user ID to check</param>
        /// <param name="depotId">The depot ID to check</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>True if user has access, false otherwise</returns>
        Task<bool> HasDepotAccessAsync(string userId, long depotId, CancellationToken cancellationToken = default);

        /// <summary>
        /// Returns the depot IDs the current user is allowed to access,
        /// or null when the user has unrestricted access (SuperAdmin / Depots.ViewAll).
        /// Scoped users get their UserDepot assignments; users with no depot permissions get an empty list.
        /// </summary>
        Task<List<long>?> GetUserAccessibleDepotIdsAsync(CancellationToken cancellationToken = default);
    }
}
