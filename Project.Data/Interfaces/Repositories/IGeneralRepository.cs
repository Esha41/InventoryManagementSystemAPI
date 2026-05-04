using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace Ettad.Data.Interfaces.Repositories
{
    public interface IGeneralRepository<T> where T : class
    {
        Task<T> GetByIdAsync(long Id);
        Task<IEnumerable<T>> GetAllAsync();
        Task<T> AddAsync(T entity);
        Task<IEnumerable<T>> AddRangeAsync(IEnumerable<T> entities);
        Task<T> UpdateAsync(T entity);
        Task DeleteAsync(T entity);
        Task<T> GetLastOrDefaultAsync<TKey>(Expression<Func<T, TKey>> keySelector);
        public Task<T> GetEntityByPropertyWithIncludeAsync(Expression<Func<T, bool>> attributeSelector, params Expression<Func<T, object>>[] includes);
        IQueryable<T> Find(Expression<Func<T, bool>> predicate, bool includeSoftDeleted = false, params string[] includes);
        Task<T> FindOneAsync(Expression<Func<T, bool>> predicate, bool includeSoftDeleted = false, params string[] includesPaths);
        Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate, bool includeSoftDeleted = false, params string[] includes);
    }
}
