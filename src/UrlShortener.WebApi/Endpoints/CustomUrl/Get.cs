using Ardalis.ApiEndpoints;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using Swashbuckle.AspNetCore.Annotations;
using UrlShortener.Core.Contracts.V1;
using UrlShortener.WebApi.Models.App;
using UrlShortener.WebApi.Services;

namespace UrlShortener.WebApi.Endpoints.CustomUrl;

public class Get : EndpointBaseAsync.WithRequest<string>.WithActionResult
{
    private readonly AppDbContext _context;
    private readonly IDistributedCache _cache;
    
    public Get(AppDbContext context, IDistributedCache cache)
    {
        _context = context;
        _cache = cache;
    }
    
    [HttpGet(ApiRoutes.CustomUrl.Get)]
    [SwaggerOperation(Tags = new[] {"CustomUrl Endpoint"})]
    public override async Task<ActionResult> HandleAsync([FromQuery] string dto, CancellationToken ct = default)
    {
        if (string.IsNullOrWhiteSpace(dto))
            return BadRequest();

        var cacheHit = await _cache.GetValueAsync<string>(dto);
        if (!string.IsNullOrWhiteSpace(cacheHit))
            return Ok(cacheHit);

        var dbHit = await _context.CustomUrls.FirstOrDefaultAsync(a => a.ShortUrl == dto, ct);

        if (dbHit is null)
            return NotFound();

        await _cache.SetValueAsync(dbHit.ShortUrl, dbHit.FullUrl);
        return Ok(dbHit.FullUrl);
    }
}