using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using UrlShortener.Contracts.V2;
using UrlShortener.Data;

namespace UrlShortener.Controllers.V2;

[ApiController]
[ApiVersion("2")]
public class SQLController : ControllerBase
{
    private readonly AppDbContext _context;
    private readonly IDistributedCache _cache;

    public SQLController([FromServices] AppDbContext context, [FromServices] IDistributedCache cache)
    {
        _context = context;
        _cache = cache;
    }

    [HttpGet(ApiRoutes.SQL.Get)]
    public async Task<IActionResult> Get(string key)
    {
        string? cacheHit = await _cache.GetValueAsync<string?>(key);

        if (!string.IsNullOrEmpty(cacheHit))
            return Ok(cacheHit);

        var dbHit = _context.Settings.FirstOrDefaultAsync(a => a.Name == key).Result;

        if (dbHit is null)
            return NotFound();

        await _cache.SetValueAsync(key, dbHit.Name, absoluteExpireTime: TimeSpan.FromMinutes(5));
        return Ok(dbHit.Name);
    }

    [HttpPost(ApiRoutes.SQL.Save)]
    [ProducesResponseType(201)]
    public async Task<IActionResult> Save(string key, string value)
    {
        if (string.IsNullOrWhiteSpace(key) || string.IsNullOrWhiteSpace(value))
            return BadRequest();

        var obj = new SettingsDataModel
        {
            Name = key,
            Value = value
        };

        await _context.Settings.AddAsync(obj);
        await _context.SaveChangesAsync();
        await _cache.SetValueAsync(key, value, absoluteExpireTime: TimeSpan.FromMinutes(5));

        return new StatusCodeResult(201);
    }
}
