using HashidsNet;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using UrlShortener.Contracts.V1;
using UrlShortener.Data;
using UrlShortener.Entities;
using UrlShortener.Services;

namespace UrlShortener.Controllers.V1;

[ApiController]
[ApiVersion("1")]
public class UrlController : ControllerBase
{
    private readonly AppDbContext _context;
    private readonly IDistributedCache _cache;
    private readonly IHashids _hashids;

    public UrlController(AppDbContext context, IDistributedCache cache, IHashids hashids)
    {
        _context = context;
        _cache = cache;
        _hashids = hashids;
    }

    [ProducesResponseType(200)]
    [ProducesResponseType(400)]
    [ProducesResponseType(404)]
    [HttpGet(ApiRoutes.Url.Get)]
    public async Task<IActionResult> Get(string shortUrl)
    {
        if (shortUrl.Length != 7)
            return BadRequest();

        var rawId = _hashids.Decode(shortUrl);

        if (rawId.Length == 0)
            return NotFound();

        var cacheHit = await _cache.GetValueAsync<string>(rawId[0].ToString());
        if (!string.IsNullOrWhiteSpace(cacheHit))
            return Ok(cacheHit);

        var dbHit = await _context.Urls.FirstOrDefaultAsync(a => a.Id == rawId[0]);

        if (dbHit is null)
            return NotFound();

        await _cache.SetValueAsync(dbHit.Id.ToString(), dbHit.FullUrl);
        return Ok(dbHit.FullUrl);
    }

    [ProducesResponseType(201)]
    [HttpPost(ApiRoutes.Url.Save)]
    public async Task<IActionResult> Save(string url)
    {
        Url obj = new()
        {
            FullUrl = url
        };

        await _context.Urls.AddAsync(obj);
        var saved = await _context.SaveChangesAsync();

        var encodedId = _hashids.EncodeLong(obj.Id);

        var createdObj = new ObjectResult(encodedId)
        {
            StatusCode = StatusCodes.Status201Created
        };

        return saved > 0 ? createdObj : StatusCode(500);
    }
}
