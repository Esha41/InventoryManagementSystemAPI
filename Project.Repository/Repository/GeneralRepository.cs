using Microsoft.EntityFrameworkCore;
using Ettad.Data.IGenericRepository_IUOW;
using Ettad.EntityFramework.DataBaseContext;
using System.Linq.Expressions;

namespace Ettad.Repository.Repository
{
    public class GeneralRepository<T> : IGeneralRepository<T> where T : class
    {
        #region fields
        protected ApplicationDbContext _context;
        DbSet<T> _entity;
        #endregion

        #region ctor
        public GeneralRepository(ApplicationDbContext context)
        {
            _context = context;
            _entity = _context.Set<T>();
        }
        #endregion

        #region Add entity async
        public async Task<T> AddAsync(T entity)
        {
            await _entity.AddAsync(entity);
            _context.SaveChanges();

            
            return entity;
        }

        public async Task<IEnumerable<T>> AddRangeAsync(IEnumerable<T> entities)
        {
            await _entity.AddRangeAsync(entities);
            _context.SaveChanges();
            return entities;
        }
        #endregion

        #region Get all entities async
        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await _entity.ToListAsync();
        }
        #endregion

        #region  Get entity by ID async
        public async Task<T> GetByIdAsync(long Id)
        {
            return await _entity.FindAsync(Id);
        }
        #endregion

        #region  Update entity async
        public async Task<T> UpdateAsync(T entity)
        {
            _entity.Attach(entity);
            _context.Entry(entity).State = EntityState.Modified;
            _context.SaveChanges();
            return entity;
        }
        #endregion

        #region Delete entity async
        public async Task DeleteAsync(T entity)
        {
            var entry = _context.Entry(entity);
            
            
            if (entry.State == EntityState.Detached)
            {
               
                var entityType = _context.Model.FindEntityType(typeof(T));
                if (entityType != null)
                {
                    var key = entityType.FindPrimaryKey();
                    if (key != null && key.Properties.Count == 1)
                    {
                        var keyProperty = key.Properties[0];
                        var propertyInfo = typeof(T).GetProperty(keyProperty.Name);
                        
                        if (propertyInfo != null)
                        {
                            var keyValue = propertyInfo.GetValue(entity);
                            
                            if (keyValue != null)
                            {
                          
                                try
                                {
                                    var trackedEntity = await _context.FindAsync<T>(keyValue);
                                    if (trackedEntity != null)
                                    {
                                        
                                        _context.Entry(trackedEntity).State = EntityState.Deleted;
                                        _context.SaveChanges();
                                        return;
                                    }
                                }
                                catch
                                {
                                  
                                }
                            }
                        }
                    }
                }
                
                try
                {
                    _entity.Remove(entity);
                }
                catch (InvalidOperationException)
                {
                    entry.State = EntityState.Deleted;
                }
            }
            else
            {
                
                entry.State = EntityState.Deleted;
            }
            
            _context.SaveChanges();
        }
        #endregion

        #region GetLastOrDefaultAsync
        public async Task<T> GetLastOrDefaultAsync<TKey>(System.Linq.Expressions.Expression<Func<T, TKey>> keySelector)
        {
            return await _entity.OrderByDescending(keySelector).FirstOrDefaultAsync();

        }
        #endregion

        #region GetEntityByPropertyWithInclude
        public async Task<T> GetEntityByPropertyWithIncludeAsync(Expression<Func<T, bool>> attributeSelector, params Expression<Func<T, object>>[] includes)
        {
            IQueryable<T> query = _context.Set<T>();

            foreach (var include in includes)
            {
                query = query.Include(include);
            }

            return await query.SingleOrDefaultAsync(attributeSelector);
        }
        #endregion
        public async Task<T> FindOneAsync(Expression<Func<T, bool>> predicate, bool includeSoftDeleted = false, params string[] includesPaths)
        {
            var items = _entity.AsNoTracking().AsQueryable();
            if (includesPaths != null)
            {
                foreach (var include in includesPaths)
                {
                    items = items.Include(include);
                }
            }
            return includeSoftDeleted ? await items.AsNoTracking().IgnoreQueryFilters().AsSplitQuery().FirstOrDefaultAsync(predicate) : await items.AsNoTracking().AsSplitQuery().FirstOrDefaultAsync(predicate);
        }

        //public IQueryable<T> Find(Expression<Func<T, bool>> predicate, bool includeSoftDeleted = false, params Expression<Func<T, object>>[] includes)
        //{
        //    var items = _entitySet.AsNoTracking().AsQueryable<T>();

        //    if (includes != null)
        //    {
        //        foreach (var include in includes)
        //        {
        //            items = items.Include(include);
        //        }
        //    }
        //    return includeSoftDeleted ? items.IgnoreQueryFilters().Where(predicate) : items.Where(predicate);
        //}

        public IQueryable<T> Find(Expression<Func<T, bool>> predicate, bool includeSoftDeleted = false, params string[] includes)
        {
            var items = _entity.AsNoTracking().AsQueryable<T>();

            if (includes != null)
            {
                foreach (var include in includes)
                {
                    items = items.Include(include);
                }
                items = items.AsSplitQuery();
            }
            return includeSoftDeleted ? items.IgnoreQueryFilters().Where(predicate) : items.Where(predicate);
        }

        public async Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate, bool includeSoftDeleted = false, params string[] includes)
        {
            IQueryable<T> items = new List<T>().AsQueryable();
            try
            {
                items = _entity.AsNoTracking().AsQueryable<T>();

                if (includes != null)
                {
                    foreach (var include in includes)
                    {
                        items = items.Include(include);
                    }
                }
                return includeSoftDeleted ? await items.IgnoreQueryFilters().Where(predicate).AsSplitQuery().ToListAsync() : await items.Where(predicate).AsSplitQuery().ToListAsync();

            }
            catch (Exception exception)
            {
                return new List<T>();
            }
        }
    }
}
