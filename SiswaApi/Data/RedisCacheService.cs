using StackExchange.Redis;
using System.Text.Json;

namespace SiswaApi.Data
{
    public class RedisCacheService
    {
        private readonly IDatabase _cache;

        public RedisCacheService(IConnectionMultiplexer redis)
        {
            _cache = redis.GetDatabase();
        }

        public async Task SetCacheAsync<T>(string key, T value, TimeSpan expiration)
        {
            var serializedData = JsonSerializer.Serialize(value);
            await _cache.StringSetAsync(key, serializedData, expiration);
        }

        public async Task<T?> GetCacheAsync<T>(string key)
        {
            var data = await _cache.StringGetAsync(key);
            if (data.IsNullOrEmpty)
                return default;

            return JsonSerializer.Deserialize<T>(data);
        }
    }
}
