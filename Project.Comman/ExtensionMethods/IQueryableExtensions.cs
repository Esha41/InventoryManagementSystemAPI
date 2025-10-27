using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace Ettad.CrossCutting.Comman.ExtensionMethods
{
    public static class IQueryableExtensions
    {
        public static int CountWithNoLock<T>(this IQueryable<T> query)
        {
            var result = 0;
            using (var scope = CreateTrancation())
            {
                result = query.Count();
                scope.Complete();
            }
            return result;
        }

        public static int CountWithNoLock<T>(
            this IQueryable<T> query,
            Expression<Func<T, bool>> predicate)
        {
            var result = 0;
            using (var scope = CreateTrancation())
            {
                result = query.Count(predicate);
                scope.Complete();
            }
            return result;
        }

        public static bool AnyWithNoLock<T>(this IQueryable<T> query)
        {
            var result = false;
            using (var scope = CreateTrancation())
            {
                result = query.Any();
                scope.Complete();
            }
            return result;
        }

        public static bool AnyWithNoLock<T>(
            this IQueryable<T> query,
            Expression<Func<T, bool>> predicate)
        {
            var result = false;
            using (var scope = CreateTrancation())
            {
                result = query.Any(predicate);
                scope.Complete();
            }
            return result;
        }

        public static T? FirstOrDefaultWithNoLock<T>(this IQueryable<T> query)
        {
            var result = default(T);
            using (var scope = CreateTrancation())
            {
                result = query.FirstOrDefault();
                scope.Complete();
            }
            return result;
        }

        public static T? FirstOrDefaultWithNoLock<T>(
            this IQueryable<T> query,
            Expression<Func<T, bool>> predicate)
        {
            var result = default(T);
            using (var scope = CreateTrancation())
            {
                result = query.FirstOrDefault(predicate);
                scope.Complete();
            }
            return result;
        }

        public static T[] ToArrayWithNoLock<T>(this IQueryable<T> query)
        {
            var result = Array.Empty<T>();
            using (var scope = CreateTrancation())
            {
                result = query.ToArray();
                scope.Complete();
            }
            return result;
        }

        public static List<T> ToListWithNoLock<T>(this IQueryable<T> query)
        {
            var result = new List<T>();
            using (var scope = CreateTrancation())
            {
                result = query.ToList();
                scope.Complete();
            }
            return result;
        }

        public static async Task<int> CountWithNoLockAsync<T>(this IQueryable<T> query)
        {
            var result = 0;
            using (var scope = CreateTrancationAsync())
            {
                result = await query.CountAsync();
                scope.Complete();
            }
            return result;
        }

        public static async Task<int> CountWithNoLockAsync<T>(
            this IQueryable<T> query,
            CancellationToken cancellationToken = new CancellationToken())
        {
            var result = 0;
            using (var scope = CreateTrancationAsync())
            {
                result = await query.CountAsync(cancellationToken);
                scope.Complete();
            }
            return result;
        }

        public static async Task<int> CountWithNoLockAsync<T>(
            this IQueryable<T> query,
            Expression<Func<T, bool>> predicate)
        {
            var result = 0;
            using (var scope = CreateTrancationAsync())
            {
                result = await query.CountAsync(predicate);
                scope.Complete();
            }
            return result;
        }

        public static async Task<int> CountWithNoLockAsync<T>(
           this IQueryable<T> query,
           Expression<Func<T, bool>> predicate,
           CancellationToken cancellationToken = new CancellationToken())
        {
            var result = 0;
            using (var scope = CreateTrancationAsync())
            {
                result = await query.CountAsync(predicate, cancellationToken);
                scope.Complete();
            }
            return result;
        }


        public static async Task<bool> AnyWithNoLockAsync<T>(this IQueryable<T> query)
        {
            var result = false;
            using (var scope = CreateTrancationAsync())
            {
                result = await query.AnyAsync();
                scope.Complete();
            }
            return result;
        }

        public static async Task<bool> AnyWithNoLockAsync<T>(
            this IQueryable<T> query,
            CancellationToken cancellationToken = new CancellationToken())
        {
            var result = false;
            using (var scope = CreateTrancationAsync())
            {
                result = await query.AnyAsync(cancellationToken);
                scope.Complete();
            }
            return result;
        }

        public static async Task<bool> AnyWithNoLockAsync<T>(
            this IQueryable<T> query,
            Expression<Func<T, bool>> predicate)
        {
            var result = false;
            using (var scope = CreateTrancationAsync())
            {
                result = await query.AnyAsync(predicate);
                scope.Complete();
            }
            return result;
        }

        public static async Task<bool> AnyWithNoLockAsync<T>(
            this IQueryable<T> query,
            Expression<Func<T, bool>> predicate,
            CancellationToken cancellationToken = new CancellationToken())
        {
            var result = false;
            using (var scope = CreateTrancationAsync())
            {
                result = await query.AnyAsync(predicate, cancellationToken);
                scope.Complete();
            }
            return result;
        }

        public static async Task<T?> FirstOrDefaultWithNoLockAsync<T>(this IQueryable<T> query)
        {
            var result = default(T);
            using (var scope = CreateTrancationAsync())
            {
                result = await query.FirstOrDefaultAsync();
                scope.Complete();
            }
            return result;
        }

        public static async Task<T?> FirstOrDefaultWithNoLockAsync<T>(
            this IQueryable<T> query,
            CancellationToken cancellationToken = new CancellationToken())
        {
            var result = default(T);
            using (var scope = CreateTrancationAsync())
            {
                result = await query.FirstOrDefaultAsync(cancellationToken);
                scope.Complete();
            }
            return result;
        }

        public static async Task<T?> FirstOrDefaultWithNoLockAsync<T>(
            this IQueryable<T> query,
            Expression<Func<T, bool>> predicate)
        {
            var result = default(T);
            using (var scope = CreateTrancationAsync())
            {
                result = await query.FirstOrDefaultAsync(predicate);
                scope.Complete();
            }
            return result;
        }

        public static async Task<T?> FirstOrDefaultWithNoLockAsync<T>(
            this IQueryable<T> query,
            Expression<Func<T, bool>> predicate,
            CancellationToken cancellationToken = new CancellationToken())
        {
            var result = default(T);
            using (var scope = CreateTrancationAsync())
            {
                result = await query.FirstOrDefaultAsync(predicate, cancellationToken);
                scope.Complete();
            }
            return result;
        }

        public static async Task<T[]> ToArrayWithNoLockAsync<T>(this IQueryable<T> query)
        {
            var result = Array.Empty<T>();
            using (var scope = CreateTrancationAsync())
            {
                result = await query.ToArrayAsync();
                scope.Complete();
            }
            return result;
        }

        public static async Task<T[]> ToArrayWithNoLockAsync<T>(
            this IQueryable<T> query,
            CancellationToken cancellationToken = new CancellationToken())
        {
            var result = Array.Empty<T>();
            using (var scope = CreateTrancationAsync())
            {
                result = await query.ToArrayAsync(cancellationToken);
                scope.Complete();
            }
            return result;
        }

        public static async Task<List<T>> ToListWithNoLockAsync<T>(this IQueryable<T> query)
        {
            var result = new List<T>();
            using (var scope = CreateTrancationAsync())
            {
                result = await query.ToListAsync();
                scope.Complete();
            }
            return result;
        }

        public static async Task<List<T>> ToListWithNoLockAsync<T>(
            this IQueryable<T> query,
            CancellationToken cancellationToken = new CancellationToken())
        {
            var result = new List<T>();
            using (var scope = CreateTrancationAsync())
            {
                result = await query.ToListAsync(cancellationToken);
                scope.Complete();
            }
            return result;
        }

        public static IQueryable<T> ApplyFilter<T>(this IQueryable<T> query, string propertyName, string filterValue)
        {
            var param = Expression.Parameter(typeof(T), "e");
            var body = GetExpressionBody(propertyName, param);
            body = Expression.Call(body, "ToLower", Type.EmptyTypes);
            body = Expression.Call(
                typeof(DbFunctionsExtensions),
                "Like",
                 Type.EmptyTypes,
                Expression.Constant(EF.Functions),
                body,
                Expression.Constant($"{filterValue}%".ToLower()));
            var lambda = Expression.Lambda(body, param);
            var queryExpr = Expression.Call(
                typeof(Queryable),
                "Where",
                new[] { typeof(T) },
                query.Expression,
                lambda);
            return query.Provider.CreateQuery<T>(queryExpr);
        }

        public static IOrderedQueryable<T> SortAsc<T>(this IQueryable<T> query, string propertyName)
        {
            return CallOrderedQueryable(query, "OrderBy", propertyName);
        }

        public static IOrderedQueryable<T> SortDesc<T>(this IQueryable<T> query, string propertyName)
        {
            return CallOrderedQueryable(query, "OrderByDescending", propertyName);
        }

        private static IOrderedQueryable<T> CallOrderedQueryable<T>(
            this IQueryable<T> query,
            string methodName,
            string propertyName)
        {
            var param = Expression.Parameter(typeof(T), "x");
            var body = GetExpressionBody(propertyName, param);
            return (IOrderedQueryable<T>)query.Provider.CreateQuery(Expression.Call(
                typeof(Queryable),
                methodName,
                new[] { typeof(T), body.Type },
                query.Expression,
                Expression.Lambda(body, param)));
        }

        private static Expression GetExpressionBody(string propertyName, ParameterExpression param)
        {
            return propertyName.Split('.').Aggregate<string, Expression>(param, Expression.PropertyOrField);
        }

        private static TransactionScope CreateTrancation()
        {
            return new TransactionScope(
                TransactionScopeOption.Required,
                new TransactionOptions()
                {
                    IsolationLevel = IsolationLevel.ReadUncommitted
                });
        }

        private static TransactionScope CreateTrancationAsync()
        {
            return new TransactionScope(
                TransactionScopeOption.Required,
                new TransactionOptions()
                {
                    IsolationLevel = IsolationLevel.ReadUncommitted
                },
                TransactionScopeAsyncFlowOption.Enabled);
        }
    }

}
