using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using System.Security.Claims;
using UrlShortener.Core.Contracts.V1;
using UrlShortener.Core.Models.Requests;
using UrlShortener.WebApi.Services;
using UrlShortener.WebApi.Models.App;
using UrlShortener.WebApi.Models.Entities;

namespace UrlShortener.WebApi.Controllers.V1;

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

    [ProducesResponseType(200)]
    [ProducesResponseType(404)]
    [HttpGet(ApiRoutes.CustomUrl.Get)]
    public async Task<IActionResult> Get(string shortUrl)
    {
        if (string.IsNullOrWhiteSpace(shortUrl))
            return BadRequest();

        var cacheHit = await _cache.GetValueAsync<string>(shortUrl);
        if (!string.IsNullOrWhiteSpace(cacheHit))
            return Ok(cacheHit);

        var dbHit = await _context.CustomUrls.FirstOrDefaultAsync(a => a.ShortUrl == shortUrl);

        if (dbHit is null)
            return NotFound();

        await _cache.SetValueAsync(dbHit.ShortUrl, dbHit.FullUrl);
        return Ok(dbHit.FullUrl);
    }

    [Authorize]
    [ProducesResponseType(201)]
    [ProducesResponseType(409)]
    [HttpPost(ApiRoutes.CustomUrl.Save)]
    public async Task<IActionResult> Save(CustomUrlDto dto)
    {
        var uid = HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);
        if (!int.TryParse(uid, out var userId))
            return Unauthorized();

        var existInDb = await _context.CustomUrls.AnyAsync(e => e.ShortUrl == dto.ShortUrl);
        if (existInDb)
            return Conflict();

        var obj = new CustomUrl()
        {
            FullUrl = dto.FullUrl,
            ShortUrl = dto.ShortUrl,
            UserId = userId
        };

        await _context.AddAsync(obj);
        var saved = await _context.SaveChangesAsync();

        return saved > 0 ? new StatusCodeResult(201) : StatusCode(500);
    }
}
