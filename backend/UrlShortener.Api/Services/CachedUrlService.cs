using StackExchange.Redis;

namespace UrlShortener.Api.Services;

public class CachedUrlService : IUrlService
{
    private readonly IUrlService _urlService;
    private readonly IConnectionMultiplexer _connectionMultiplexer;

    public CachedUrlService(IUrlService urlService, IConnectionMultiplexer connectionMultiplexer)
    {
        _urlService = urlService;
        _connectionMultiplexer = connectionMultiplexer;
    }

    public async Task<string?> GetUrlByIdAsync(int id, CancellationToken ct = default)
    {
        var database = _connectionMultiplexer.GetDatabase();

        var cacheHit = await database.StringGetAsync(id.ToString());

        if (cacheHit.HasValue)
            return cacheHit;

        var dbHit = await _urlService.GetUrlByIdAsync(id, ct);

        if (dbHit is null)
            return null;

        await database.StringSetAsync(id.ToString(), dbHit);
        return dbHit;
    }

    public async Task<string?> GetCustomUrlAsync(string shortUrl, CancellationToken ct = default)
    {
        var database = _connectionMultiplexer.GetDatabase();

        var cacheHit = await database.StringGetAsync(shortUrl);

        if (cacheHit.HasValue)
            return cacheHit;

        var dbHit = await _urlService.GetCustomUrlAsync(shortUrl, ct);

        if (dbHit is null)
            return null;

        await database.StringSetAsync(shortUrl, dbHit);
        return dbHit;
    }
}