using HashidsNet;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using System.Security.Claims;
using UrlShortener.Contracts.V1;
using UrlShortener.Data;
using UrlShortener.Entities;
using UrlShortener.Services;

namespace UrlShortener.Controllers.V1;

[Authorize]
[ApiController]
[ApiVersion("1")]
public class CustomUrlController : ControllerBase
{
    private readonly AppDbContext _context;
    private readonly IDistributedCache _cache;

    public CustomUrlController(AppDbContext context, IDistributedCache cache)
    {
        _context = context;
        _cache = cache;
    }

    [HttpGet(ApiRoutes.CustomUrl.Get)]
    public async Task<IActionResult> Get(string shortUrl)
    {
        if (string.IsNullOrWhiteSpace(shortUrl))
            return BadRequest();

        var cacheHit = await _cache.GetValueAsync<string>(shortUrl);
        if (!string.IsNullOrEmpty(cacheHit))
            return Ok(cacheHit);

        var dbHit = await _context.CustomUrl.FirstOrDefaultAsync(a => a.ShortUrl == shortUrl);

        if (dbHit is null)
            return NotFound();

        await _cache.SetValueAsync(dbHit.ShortUrl, dbHit.FullUrl);
        return Ok(dbHit.FullUrl);
    }

    [HttpPost(ApiRoutes.CustomUrl.Save)]
    [ProducesResponseType(201)]
    public async Task<IActionResult> Save(string fullUrl, string customUrl)
    {
        if (string.IsNullOrWhiteSpace(fullUrl) || string.IsNullOrWhiteSpace(customUrl))
            return BadRequest();

        var uid = HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (!int.TryParse(uid, out var id))
            return Unauthorized();

        var obj = new CustomUrl()
        {
            FullUrl = fullUrl,
            ShortUrl = customUrl,
            UserId = id
        };

        await _context.AddAsync(obj);
        await _context.SaveChangesAsync();

        return new StatusCodeResult(201);
    }
}
