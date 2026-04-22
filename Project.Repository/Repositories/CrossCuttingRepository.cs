using Ettad.Data.Interfaces.Repositories;
using Ettad.EntityFramework.DataBaseContext;

namespace Ettad.Repository.Repositories
{
    public class CrossCuttingRepository<T> : GeneralRepository<T>, ICrossCuttingRepository<T> where T : class
    {
        public CrossCuttingRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}
