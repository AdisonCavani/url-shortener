using Microsoft.Extensions.Caching.Distributed;
using UrlShortener.WebApi.Services.Interfaces;

namespace UrlShortener.WebApi.Services;

public class CachedUrlService : IUrlService
{
    private readonly IDistributedCache _cache;
    private readonly IUrlService _urlService;

    public CachedUrlService(IDistributedCache cache, IUrlService urlService)
    {
        _urlService = urlService;
        _cache = cache;
    }

    public async Task<string?> GetUrlByIdAsync(int id, CancellationToken ct = default)
    {
        // var result = await _memoryCache.GetOrCreateAsync(id,
        //     async entry =>
        //     {
        //         entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(2);
        //         return await _urlService.GetUrlByIdAsync(id);
        //     });
        //
        // return result;

        var cacheHit = await _cache.GetValueAsync<string>(id.ToString(), ct: ct);

        if (cacheHit is not null)
            return cacheHit;

        var dbHit = await _urlService.GetUrlByIdAsync(id, ct);

        if (dbHit is null)
            return null;

        await _cache.SetValueAsync(id.ToString(), dbHit, ct: ct);
        return dbHit;
    }

    public async Task<string?> GetCustomUrlAsync(string shortUrl, CancellationToken ct = default)
    {
        var cacheHit = await _cache.GetValueAsync<string>(shortUrl, ct);

        if (cacheHit is not null)
            return cacheHit;

        var dbHit = await _urlService.GetCustomUrlAsync(shortUrl, ct);

        if (dbHit is null)
            return null;

        await _cache.SetValueAsync(shortUrl, dbHit, ct: ct);
        return dbHit;
    }
}