using Ettad.Data.Interfaces.Repositories;
using Ettad.EntityFramework.DataBaseContext;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace Ettad.Repository.Repositories
{
    public class CrossCuttingRepository<T> : GeneralRepository<T>, ICrossCuttingRepository<T> where T : class
    {
        public CrossCuttingRepository(ApplicationDbContext context) : base(context)
        {
        }

        /// <summary>
        /// Same <see cref="DatabaseFacade"/> as all repositories sharing this DbContext instance (for scoped transactions).
        /// </summary>
        public DatabaseFacade Database => _context.Database;
    }
}
