using System.Threading;

namespace Ettad.CrossCutting.Data.Repository
{
    public interface ITransactionManager
    {
        Task<IAsyncDisposable> BeginAsync(CancellationToken cancellationToken = default);
        Task CommitAsync(CancellationToken cancellationToken = default);
        Task RollbackAsync(CancellationToken cancellationToken = default);
    }
}
