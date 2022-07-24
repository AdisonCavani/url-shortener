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

public class Delete : EndpointBaseAsync.WithRequest<DeleteUserUrlRequest>.WithActionResult
{
    private readonly AppDbContext _context;
    private readonly IConnectionMultiplexer _connectionMultiplexer;

    public Delete(AppDbContext context, IConnectionMultiplexer connectionMultiplexer)
    {
        _context = context;
        _connectionMultiplexer = connectionMultiplexer;
    }

    [Authorize]
    [HttpDelete(ApiRoutes.UserUrl.Delete)]
    [SwaggerOperation(Tags = new[] {"UserUrl Endpoint"})]
    public override async Task<ActionResult> HandleAsync(DeleteUserUrlRequest req, CancellationToken ct = default)

    {
        var existInDb = await _context.UserUrls.FirstOrDefaultAsync(x => x.Id == req.Id, ct);

        if (existInDb is null)
            return NotFound();

        var userId = HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);

        if (userId is null)
            return StatusCode(StatusCodes.Status500InternalServerError);

        if (existInDb.UserId != userId)
            return StatusCode(StatusCodes.Status403Forbidden);

        _context.UserUrls.Remove(existInDb);
        var result = await _context.SaveChangesAsync(ct);

        var cacheDb = _connectionMultiplexer.GetDatabase();
        await cacheDb.KeyDeleteAsync(existInDb.Id.ToString());

        return result > 0 ? Ok() : StatusCode(StatusCodes.Status500InternalServerError);
    }
}