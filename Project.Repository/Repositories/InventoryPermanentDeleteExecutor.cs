using Ettad.Data.Interfaces.Repositories;
using Ettad.EntityFramework.DataBaseContext;
using Microsoft.EntityFrameworkCore;

namespace Ettad.Repository.Repositories;

public class InventoryPermanentDeleteExecutor : IInventoryPermanentDeleteExecutor
{
    private readonly ApplicationDbContext _context;

    public InventoryPermanentDeleteExecutor(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<(int SubtypeRowsDeleted, int BaseItemRowsDeleted)> ExecuteAsync(
        InventoryPermanentDeleteKind kind,
        long baseItemId,
        CancellationToken cancellationToken = default)
    {
        await _context.Database.ExecuteSqlInterpolatedAsync(
            $"DELETE FROM BaseItemPrimaryPurposes WHERE BaseItemId = {baseItemId}",
            cancellationToken);

        var subtypeRows = kind switch
        {
            InventoryPermanentDeleteKind.Ammunition =>
                await _context.Database.ExecuteSqlInterpolatedAsync(
                    $"DELETE FROM Ammunitions WHERE Id = {baseItemId}",
                    cancellationToken),
            InventoryPermanentDeleteKind.Weapon =>
                await _context.Database.ExecuteSqlInterpolatedAsync(
                    $"DELETE FROM Weapons WHERE Id = {baseItemId}",
                    cancellationToken),
            InventoryPermanentDeleteKind.Explosive =>
                await _context.Database.ExecuteSqlInterpolatedAsync(
                    $"DELETE FROM Explosives WHERE Id = {baseItemId}",
                    cancellationToken),
            _ => throw new ArgumentOutOfRangeException(nameof(kind), kind, null)
        };

        var baseRows = await _context.Database.ExecuteSqlInterpolatedAsync(
            $"DELETE FROM BaseItems WHERE Id = {baseItemId}",
            cancellationToken);

        return (subtypeRows, baseRows);
    }
}
