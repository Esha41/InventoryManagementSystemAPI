using Ettad.Data.Enums;

namespace Ettad.Data.Interfaces.Repositories;


/// <summary>
/// Physically deletes inventory rows for ammunition, weapons, and explosives (purpose links, type-specific row, BaseItems row).
/// </summary>
public interface IInventoryPermanentDeleteExecutor
{
    /// <summary>
    /// Deletes primary-purpose links, item-specific row, then BaseItems for the given id (FK-safe order).
    /// Returns deleted row counts from type-specific tables and BaseItems respectively.
    /// </summary>
    Task<(int SubtypeRowsDeleted, int BaseItemRowsDeleted)> ExecuteAsync(
        InventoryPermanentDeleteKind kind,
        long baseItemId,
        CancellationToken cancellationToken = default);
}
