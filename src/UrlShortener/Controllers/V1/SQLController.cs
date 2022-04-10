using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using UrlShortener.Contracts.V1;
using UrlShortener.Data;

namespace UrlShortener.Controllers.V1;

[ApiController]
[ApiVersion("1")]
public class SQLController : ControllerBase
{
    private readonly AppDbContext _context;
    private readonly IDistributedCache _cache;

    public SQLController(AppDbContext context, IDistributedCache cache)
    {
        _context = context;
        _cache = cache;
    }

    [HttpGet(ApiRoutes.SQL.Get)]
    public async Task<IActionResult> Get(string key)
    {
        var cacheHit = await _cache.GetValueAsync<string?>(key);

        if (!string.IsNullOrEmpty(cacheHit))
            return Ok(cacheHit);

        var dbHit = await _context.Settings.FirstOrDefaultAsync(a => a.Name == key);

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
