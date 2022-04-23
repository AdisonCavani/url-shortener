using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;

namespace UrlShortener.Services;

public static class DistributedCacheExtensions
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="cache"></param>
    /// <param name="key"></param>
    /// <returns></returns>
    public static async Task<T?> GetValueAsync<T>(this IDistributedCache cache, string key)
    {
        var jsonData = await cache.GetStringAsync(key);

        return jsonData is null ? default : JsonSerializer.Deserialize<T>(jsonData);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="cache"></param>
    /// <param name="key"></param>
    /// <param name="value"></param>
    /// <param name="absoluteExpireTime">60 seconds by default</param>
    /// <param name="unusedExpireTime"></param>
    /// <returns></returns>
    public static async Task SetValueAsync<T>(this IDistributedCache cache,
        string key,
        T value,
        TimeSpan? absoluteExpireTime = null,
        TimeSpan? unusedExpireTime = null)
    {
        var jsonData = JsonSerializer.Serialize(value);
        await cache.SetStringAsync(key, jsonData, new DistributedCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = absoluteExpireTime ?? TimeSpan.FromSeconds(60),
            SlidingExpiration = unusedExpireTime
        });
    }
}
