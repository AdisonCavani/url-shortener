using Ardalis.ApiEndpoints;
using HashidsNet;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using Swashbuckle.AspNetCore.Annotations;
using UrlShortener.Core.Contracts.V1;
using UrlShortener.WebApi.Models.App;
using UrlShortener.WebApi.Services;

namespace UrlShortener.WebApi.Endpoints.Url;

public class Get : EndpointBaseAsync.WithRequest<string>.WithActionResult
{
    private readonly AppDbContext _context;
    private readonly IHashids _hashids;
    private readonly IDistributedCache _cache;

    public Get(AppDbContext context, IHashids hashids, IDistributedCache cache)
    {
        _context = context;
        _hashids = hashids;
        _cache = cache;
    }

    [HttpGet(ApiRoutes.Url.Get)]
    [SwaggerOperation(Tags = new[] {"Url Endpoint"})]
    public override async Task<ActionResult> HandleAsync([FromQuery] string dto, CancellationToken ct = default)
    {
        if (dto.Length != 7)
            return BadRequest();

        var rawId = _hashids.Decode(dto);

        if (rawId.Length == 0)
            return NotFound();

        var cacheHit = await _cache.GetValueAsync<string>(rawId[0].ToString());
        if (!string.IsNullOrWhiteSpace(cacheHit))
            return Ok(cacheHit);

        var dbHit = await _context.Urls.FirstOrDefaultAsync(a => a.Id == rawId[0], ct);

        if (dbHit is null)
            return NotFound();

        await _cache.SetValueAsync(dbHit.Id.ToString(), dbHit.FullUrl);
        return Ok(dbHit.FullUrl);
    }
}