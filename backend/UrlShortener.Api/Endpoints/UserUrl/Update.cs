using System.Security.Claims;
using Ardalis.ApiEndpoints;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StackExchange.Redis;
using Swashbuckle.AspNetCore.Annotations;
using UrlShortener.Api.Database;
using UrlShortener.Api.Mapping;
using UrlShortener.Shared.Contracts;
using UrlShortener.Shared.Contracts.Requests;

namespace UrlShortener.Api.Endpoints.UserUrl;

public class Update : EndpointBaseAsync.WithRequest<UpdateUserUrlRequest>.WithActionResult
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
    [SwaggerOperation(Tags = new[] {"UserUrl Endpoint"})]
    public override async Task<ActionResult> HandleAsync(UpdateUserUrlRequest req, CancellationToken ct = default)
    {
        var userId = HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);

        if (userId is null)
            return StatusCode(StatusCodes.Status500InternalServerError);
        
        var existInDb = await _context.Urls
            .Include(x => x.UrlDetails)
            .Include(x => x.UrlDetails!.Tags)
            .FirstOrDefaultAsync(x => x.Id == req.Id, ct);

        if (existInDb is null)
            return NotFound();

        if (existInDb.UrlDetails?.UserId != userId)
            return Forbid();

        existInDb.FullUrl = req.Url;
        existInDb.UrlDetails.Title = req.Title;

        if (existInDb.UrlDetails.Tags is not null)
            _context.Tags.RemoveRange(existInDb.UrlDetails.Tags);

        existInDb.UrlDetails.Tags = req.Tags?.ToTagEntityList();
        var result = await _context.SaveChangesAsync(ct);

        var cacheDb = _connectionMultiplexer.GetDatabase();
        await cacheDb.KeyDeleteAsync(existInDb.Id.ToString());

        return result > 0 ? Ok() : StatusCode(StatusCodes.Status304NotModified);
    }
}