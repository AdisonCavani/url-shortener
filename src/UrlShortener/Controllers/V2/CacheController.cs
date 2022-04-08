using Microsoft.AspNetCore.Mvc;
using UrlShortener.Contracts.V2;
using UrlShortener.Models;
using UrlShortener.Services;

namespace UrlShortener.Controllers.V2;

[ApiController]
[ApiVersion("2")]
public class CacheController : ControllerBase
{
    private readonly ICacheService _cacheService;

    public CacheController(ICacheService cacheService)
    {
        _cacheService = cacheService;
    }

    [HttpPost(ApiRoutes.Cache.SetCacheValue)]
    public async Task<IActionResult> SetCacheValue(string key, string value)
    {
        await _cacheService.SetCacheValueAsync(key, value);
        return Ok();
    }

    [HttpGet(ApiRoutes.Cache.GetCacheValue)]
    public async Task<IActionResult> GetCacheValue(string key)
    {
        var value = await _cacheService.GetCacheValueAsync(key);
        return string.IsNullOrEmpty(value) ? NotFound() : Ok(value);
    }
}
