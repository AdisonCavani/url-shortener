using StackExchange.Redis;

namespace UrlService.Services;

public class CachedUrlRepository : IUrlRepository
{
    private readonly IUrlRepository _urlRepository;
    private readonly IConnectionMultiplexer _connectionMultiplexer;

    public CachedUrlRepository(IUrlRepository urlRepository, IConnectionMultiplexer connectionMultiplexer)
    {
        _urlRepository = urlRepository;
        _connectionMultiplexer = connectionMultiplexer;
    }

    public async Task<string?> GetUrlByIdAsync(long id, CancellationToken ct = default)
    {
        var database = _connectionMultiplexer.GetDatabase();

        var cacheHit = await database.StringGetAsync(id.ToString());

        if (cacheHit.HasValue)
            return cacheHit;

        var dbHit = await _urlRepository.GetUrlByIdAsync(id, ct);

        if (dbHit is null)
            return null;

        await database.StringSetAsync(id.ToString(), dbHit);
        return dbHit;
    }
}