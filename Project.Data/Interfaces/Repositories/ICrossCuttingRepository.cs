using Ettad.Data.Interfaces.Repositories;

namespace Ettad.Data.Interfaces.Repositories
{
    public interface ICrossCuttingRepository<T> : IGeneralRepository<T> where T : class
    {
    }
}
