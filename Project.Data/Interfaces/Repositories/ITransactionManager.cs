using System.Threading;

namespace Ettad.Data.Interfaces.Repositories
{
    public interface ITransactionManager
    {
        bool HasActiveTransaction { get; }
        Task<IAsyncDisposable> BeginAsync(CancellationToken cancellationToken = default);
        Task CommitAsync(CancellationToken cancellationToken = default);
        Task RollbackAsync(CancellationToken cancellationToken = default);
    }
}
