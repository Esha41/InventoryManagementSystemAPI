using Ettad.Data.Interfaces.Repositories;
using Ettad.EntityFramework.DataBaseContext;
using Microsoft.EntityFrameworkCore.Storage;

namespace Ettad.Repository.Repositories
{
    public class EfTransactionManager : ITransactionManager
    {
        private readonly ApplicationDbContext _context;
        private IDbContextTransaction _currentTransaction;

        public EfTransactionManager(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IAsyncDisposable> BeginAsync(CancellationToken cancellationToken = default)
        {
            if (_currentTransaction != null)
                throw new InvalidOperationException("A transaction is already active for this scope.");

            _currentTransaction = await _context.Database.BeginTransactionAsync(cancellationToken);
            return _currentTransaction;
        }

        public async Task CommitAsync(CancellationToken cancellationToken = default)
        {
            if (_currentTransaction == null)
                throw new InvalidOperationException("No active transaction to commit.");

            await _currentTransaction.CommitAsync(cancellationToken);
            await _currentTransaction.DisposeAsync();
            _currentTransaction = null;
        }

        public async Task RollbackAsync(CancellationToken cancellationToken = default)
        {
            if (_currentTransaction == null)
                return;

            await _currentTransaction.RollbackAsync(cancellationToken);
            await _currentTransaction.DisposeAsync();
            _currentTransaction = null;
        }
    }
}
