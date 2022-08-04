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

    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [Authorize]
    [HttpDelete(ApiRoutes.UserUrl.Delete)]
    [SwaggerOperation(Tags = new[] {"UserUrl Endpoint"})]
    public override async Task<ActionResult> HandleAsync(DeleteUserUrlRequest req, CancellationToken ct = default)

    {
        var userId = HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);

        if (userId is null)
            return StatusCode(StatusCodes.Status500InternalServerError);
        
        var existInDb = await _context.Urls.FirstOrDefaultAsync(x => x.Id == req.Id, ct);

        if (existInDb is null)
            return NotFound();

        if (existInDb.UrlDetails?.UserId != userId)
            return Forbid();

        _context.Urls.Remove(existInDb);
        var result = await _context.SaveChangesAsync(ct);

        var cacheDb = _connectionMultiplexer.GetDatabase();
        await cacheDb.KeyDeleteAsync(existInDb.Id.ToString());

        return result > 0 ? Ok() : StatusCode(StatusCodes.Status500InternalServerError);
    }
}