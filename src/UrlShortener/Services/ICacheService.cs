namespace UrlShortener.Services;

public interface ICacheService
{
    /// <summary>
    /// Get a set from <paramref name="key"/>
    /// </summary>
    /// <param name="key">The key of set value pair</param>
    /// <returns></returns>
    Task<string> GetCacheValueAsync(string key);

    /// <summary>
    /// Insert <paramref name="key"/> and <paramref name="value"/> or update existing set
    /// </summary>
    /// <param name="key">The key to set or update</param>
    /// <param name="value">The value to set or update</param>
    /// <returns></returns>
    Task SetCacheValueAsync(string key, string value);
}
