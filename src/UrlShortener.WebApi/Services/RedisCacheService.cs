using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;

namespace UrlShortener.WebApi.Services;

public static class DistributedCacheExtensions
{
    public static async Task<T?> GetValueAsync<T>(this IDistributedCache cache, string key, CancellationToken ct = default)
    {
        var jsonData = await cache.GetStringAsync(key, ct);

        return string.IsNullOrWhiteSpace(jsonData) ? default : JsonSerializer.Deserialize<T>(jsonData);
    }

    public static async Task SetValueAsync<T>(
        this IDistributedCache cache,
        string key,
        T value,
        TimeSpan? absoluteExpireTime = null,
        TimeSpan? unusedExpireTime = null,
        CancellationToken ct = default)
    {
        var jsonData = JsonSerializer.Serialize(value);
        await cache.SetStringAsync(key, jsonData, new DistributedCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = absoluteExpireTime ?? TimeSpan.FromSeconds(60),
            SlidingExpiration = unusedExpireTime
        }, ct);
    }
}