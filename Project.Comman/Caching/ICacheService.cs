using System;
using System.Threading.Tasks;

namespace Ettad.Shared.Common.Caching
{
    public interface ICacheService
    {
        Task<T> GetOrSetAsync<T>(string key, Func<Task<T>> factory, TimeSpan? expiration = null);
        Task<T> GetAsync<T>(string key);
        Task SetAsync<T>(string key, T value, TimeSpan? expiration = null);
        Task RemoveAsync(string key);
        Task<bool> ExistsAsync(string key);
    }
} 