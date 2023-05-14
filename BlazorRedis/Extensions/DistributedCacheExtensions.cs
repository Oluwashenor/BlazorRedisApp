using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;

namespace BlazorRedis.Extensions
{
    public static class DistributedCacheExtensions
    {

        //Using "this" - means its an extension method for any where there is IDistributedCache
        public static async Task SetRecordAsync<T>(this IDistributedCache cache, string recordId, T data, TimeSpan? absoluteExpireTime = null, TimeSpan? unusedExpireTime = null)
        {
            var options = new DistributedCacheEntryOptions();
            options.AbsoluteExpirationRelativeToNow = absoluteExpireTime ?? TimeSpan.FromSeconds(60); // It will live for a total of 60 secs in cache. After Redis would delete it
            options.SlidingExpiration = unusedExpireTime;  // if the cache item is not used with the specified time, it would be deleted even if AbsoluteExpirationRelativeToNow has not been met
            var jsonData = JsonSerializer.Serialize(data);
            await cache.SetStringAsync(recordId,jsonData, options);
        }

        public static async Task<T> GetRecordAsync<T>(this IDistributedCache cache, string recordId)
        {
            var jsonData = await cache.GetStringAsync(recordId);
            if (jsonData is null) return default(T);
            return JsonSerializer.Deserialize<T>(jsonData);
        }
    }
}
