using System.Security.Claims;
using Ardalis.ApiEndpoints;
using HashidsNet;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.Annotations;
using UrlService.Database;
using UrlService.Mapping;
using UrlShortener.Shared.Contracts;
using UrlShortener.Shared.Contracts.Requests;
using UrlShortener.Shared.Contracts.Responses;

namespace UrlService.Endpoints.UserUrl;

public class Get : EndpointBaseAsync.WithRequest<GetUserUrlRequest>.WithActionResult<GetUserUrlResponse>
{
    private readonly AppDbContext _context;
    private readonly IHashids _hashids;

    public Get(AppDbContext context, IHashids hashids)
    {
        _context = context;
        _hashids = hashids;
    }

    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [Authorize]
    [HttpGet(ApiRoutes.UserUrl.Get)]
    [SwaggerOperation(Tags = new[] {"UserUrl Endpoint"})]
    public override async Task<ActionResult<GetUserUrlResponse>> HandleAsync(
        [FromQuery] GetUserUrlRequest req,
        CancellationToken ct = default)
    {
        var userId = HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);

        if (userId is null)
            return StatusCode(StatusCodes.Status500InternalServerError);

        var existInDb = await _context.Urls
            .AsNoTracking()
            .Include(x => x.UrlDetails)
            .FirstOrDefaultAsync(x => x.Id == req.Id, ct);

        if (existInDb is null)
            return NotFound();

        if (existInDb.UrlDetails?.UserId != userId)
            return Forbid();

        return Ok(new GetUserUrlResponse
        {
            Id = existInDb.Id,
            ShortUrl = _hashids.EncodeLong(existInDb.Id),
            FullUrl = existInDb.FullUrl,
            UrlDetails = existInDb.UrlDetails.ToUrlDetails()
        });
    }
}