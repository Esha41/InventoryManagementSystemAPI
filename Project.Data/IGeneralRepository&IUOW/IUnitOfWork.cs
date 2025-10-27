using Ettad.Data.Entities;
using Ettad.Comman.Idenitity;
using Ettad.Data.Entities;

namespace Ettad.Data.IGenericRepository_IUOW
{
    public interface IUnitOfWork : IDisposable
    {
     
        public IGeneralRepository<ApplicationUser> Users { get; }
     


        Task<bool> SaveAsync();
    }
}
