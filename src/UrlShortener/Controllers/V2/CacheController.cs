using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using UrlShortener.Contracts.V2;

namespace UrlShortener.Controllers.V2;

[ApiController]
[ApiVersion("2")]
public class CacheController : ControllerBase
{
    private readonly IDistributedCache _cache;

    public CacheController(IDistributedCache cache)
    {
        _cache = cache;
    }

    [HttpPost(ApiRoutes.Cache.SetCacheValue)]
    public async Task<IActionResult> SetCacheValue(string key, string value)
    {
        await _cache.SetValueAsync(key, value, unusedExpireTime: TimeSpan.FromMinutes(5));
        return Ok();
    }

    [HttpGet(ApiRoutes.Cache.GetCacheValue)]
    public async Task<IActionResult> GetCacheValue(string key)
    {
        var value = await _cache.GetValueAsync<string>(key);
        return string.IsNullOrEmpty(value) ? NotFound() : Ok(value);
    }
}
