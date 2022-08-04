using StackExchange.Redis;
using UrlService.Services.Interfaces;

namespace UrlService.Services;

public class CachedUrlService : IUrlService
{
    private readonly IUrlService _urlService;
    private readonly IConnectionMultiplexer _connectionMultiplexer;

    public CachedUrlService(IUrlService urlService, IConnectionMultiplexer connectionMultiplexer)
    {
        _urlService = urlService;
        _connectionMultiplexer = connectionMultiplexer;
    }

    public async Task<string?> GetUrlByIdAsync(long id, CancellationToken ct = default)
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
}