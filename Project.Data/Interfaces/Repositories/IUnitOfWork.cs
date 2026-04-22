using Ettad.Comman.Idenitity;

namespace Ettad.Data.Interfaces.Repositories
{
    public interface IUnitOfWork : IDisposable
    {
     
        public IGeneralRepository<ApplicationUser> Users { get; }
        Task<bool> SaveAsync();
    }
}
