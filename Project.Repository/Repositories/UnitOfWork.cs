using Ettad.Comman.Idenitity;
using Ettad.Data.Interfaces.Repositories;

namespace Ettad.Repository.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ICrossCuttingRepository<ApplicationUser> _usersRepository;

        public IGeneralRepository<ApplicationUser> Users => _usersRepository;

        public UnitOfWork(ICrossCuttingRepository<ApplicationUser> usersRepository)
        {
            _usersRepository = usersRepository;
        }

        public async Task<bool> SaveAsync()
        {
            int result = await _usersRepository.SaveChangesAsync();
            return result > 0;
        }

        public void Dispose()
        {
            // DbContext is scoped and owned by DI; do not dispose here.
        }
    }
}
