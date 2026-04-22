using Ettad.EntityFramework.DataBaseContext;
using Ettad.Comman.Idenitity;
using Ettad.Data.Interfaces.Repositories;

namespace Ettad.Repository.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _context;
     
        public IGeneralRepository<ApplicationUser> Users { get; private set; }
 
        public UnitOfWork(ApplicationDbContext context)
        {
            _context = context;
          
            Users= new GeneralRepository<ApplicationUser>(_context);
        }
        public async Task<bool> SaveAsync()
        {
            int result = await _context.SaveChangesAsync();
            return result > 0;
        }
        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
