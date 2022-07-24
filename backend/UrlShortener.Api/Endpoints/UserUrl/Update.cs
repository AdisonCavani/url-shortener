using System.Security.Claims;
using Ardalis.ApiEndpoints;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StackExchange.Redis;
using Swashbuckle.AspNetCore.Annotations;
using UrlShortener.Api.Database;
using UrlShortener.Shared.Contracts;
using UrlShortener.Shared.Contracts.Requests;

namespace UrlShortener.Api.Endpoints.UserUrl;

public class Update : EndpointBaseAsync.WithRequest<UpdateUrlRequest>.WithActionResult
{
    private readonly AppDbContext _context;
    private readonly IConnectionMultiplexer _connectionMultiplexer;

    public Update(AppDbContext context, IConnectionMultiplexer connectionMultiplexer)
    {
        _context = context;
        _connectionMultiplexer = connectionMultiplexer;
    }

    [Authorize]
    [HttpPatch(ApiRoutes.UserUrl.Update)]
    [SwaggerOperation(Tags = new[] { "UserUrl Endpoint" })]
    public override async Task<ActionResult> HandleAsync(UpdateUrlRequest req, CancellationToken ct = default)
    {
        var existInDb = await _context.UserUrls.FirstOrDefaultAsync(x => x.Id == req.Id, ct);

        if (existInDb is null)
            return NotFound();

        var userId = HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);

        if (userId is null)
            return StatusCode(StatusCodes.Status500InternalServerError);

        if (existInDb.UserId != userId)
            return StatusCode(StatusCodes.Status403Forbidden);

        existInDb.FullUrl = req.NewUrl;
        var result = await _context.SaveChangesAsync(ct);

        var cacheDb = _connectionMultiplexer.GetDatabase();
        await cacheDb.KeyDeleteAsync(existInDb.Id.ToString());

        return result > 0 ? Ok() : StatusCode(StatusCodes.Status500InternalServerError);
    }
}