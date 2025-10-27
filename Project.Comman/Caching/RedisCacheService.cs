using System;
using System.Text.Json;
using System.Threading.Tasks;
using StackExchange.Redis;

namespace Ettad.Shared.Common.Caching
{
    public class RedisCacheService : ICacheService
    {
        private readonly IConnectionMultiplexer _redis;
        private readonly IDatabase _db;

        public RedisCacheService(string connectionString)
        {
            _redis = ConnectionMultiplexer.Connect(connectionString);
            _db = _redis.GetDatabase();
        }

        public async Task<T> GetOrSetAsync<T>(string key, Func<Task<T>> factory, TimeSpan? expiration = null)
        {
            var value = await GetAsync<T>(key);
            if (value != null) return value;

            value = await factory();
            await SetAsync(key, value, expiration);
            return value;
        }

        public async Task<T> GetAsync<T>(string key)
        {
            var value = await _db.StringGetAsync(key);
            if (value.IsNull) return default;
            return JsonSerializer.Deserialize<T>(value);
        }

        public async Task SetAsync<T>(string key, T value, TimeSpan? expiration = null)
        {
            var serializedValue = JsonSerializer.Serialize(value);
            await _db.StringSetAsync(key, serializedValue, expiration);
        }

        public async Task RemoveAsync(string key)
        {
            await _db.KeyDeleteAsync(key);
        }

        public async Task<bool> ExistsAsync(string key)
        {
            return await _db.KeyExistsAsync(key);
        }
    }
} 