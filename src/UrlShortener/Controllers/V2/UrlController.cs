using HashidsNet;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using UrlShortener.Contracts.V2;
using UrlShortener.Data;

namespace UrlShortener.Controllers.V2;

[ApiController]
[ApiVersion("2")]
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

    [HttpGet(ApiRoutes.Url.Get)]
    public async Task<IActionResult> Get(string shortUrl)
    {
        if (shortUrl.Length != 7)
            return BadRequest();

        var rawId = _hashids.Decode(shortUrl);

        if (rawId.Length == 0)
            return NotFound();

        var cacheHit = await _cache.GetValueAsync<string>(rawId[0].ToString());
        if (!string.IsNullOrEmpty(cacheHit))
            return Ok(cacheHit);

        var dbHit = await _context.Url.FirstOrDefaultAsync(a => a.Id == rawId[0]);

        if (dbHit is null)
            return NotFound();

        await _cache.SetValueAsync(dbHit.Id.ToString(), dbHit.FullUrl);
        return Ok(dbHit.FullUrl);
    }

    [HttpPost(ApiRoutes.Url.Save)]
    [ProducesResponseType(201)]
    public async Task<IActionResult> Save(string url)
    {
        var obj = new UrlDataModel()
        {
            FullUrl = url
        };

        var savedObj = await _context.AddAsync(obj);
        await _context.SaveChangesAsync();

        var encodedId = _hashids.EncodeLong(savedObj.Entity.Id);

        return new ObjectResult(encodedId)
        {
            StatusCode = StatusCodes.Status201Created
        };
    }
}
