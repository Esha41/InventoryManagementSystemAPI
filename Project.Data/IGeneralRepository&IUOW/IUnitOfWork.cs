using Ettad.Comman.Idenitity;

namespace Ettad.Data.IGenericRepository_IUOW
{
    public interface IUnitOfWork : IDisposable
    {
     
        public IGeneralRepository<ApplicationUser> Users { get; }
        Task<bool> SaveAsync();
    }
}
