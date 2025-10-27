using Ettad.EntityFramework.DataBaseContext;
using Ettad.Repository.Repository;

namespace Ettad.CrossCutting.Data.Repository
{
    public class CrossCuttingRepository<T> : GeneralRepository<T>, ICrossCuttingRepository<T> where T : class
    {
        public CrossCuttingRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}
