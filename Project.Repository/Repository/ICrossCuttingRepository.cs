
using Ettad.Data.IGenericRepository_IUOW;

namespace Ettad.CrossCutting.Data.Repository
{
    public interface ICrossCuttingRepository<T> : IGeneralRepository<T> where T : class
    {
    }
}
