using System.Security.Claims;
using Ardalis.ApiEndpoints;
using HashidsNet;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using UrlShortener.Api.Database;
using UrlShortener.Shared.Contracts;
using UrlShortener.Shared.Contracts.Requests;
using UrlShortener.Shared.Contracts.Responses;

namespace UrlShortener.Api.Endpoints.Url;

public class Save : EndpointBaseAsync.WithRequest<SaveUrlRequest>.WithActionResult<SaveUrlResponse>
{
    private readonly AppDbContext _context;
    private readonly IHashids _hashids;

    public Save(AppDbContext context, IHashids hashids)
    {
        _context = context;
        _hashids = hashids;
    }

    [Authorize]
    [HttpPost(ApiRoutes.Url.Save)]
    [SwaggerOperation(Tags = new[] { "Url Endpoint" })]
    public override async Task<ActionResult<SaveUrlResponse>> HandleAsync(SaveUrlRequest req, CancellationToken ct = default)
    {
        var userId = HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);

        if (userId is null)
            return StatusCode(StatusCodes.Status500InternalServerError);
        
        Database.Entities.Url obj = new()
        {
            FullUrl = req.Url,
            UserId = userId
        };

        await _context.Urls.AddAsync(obj, ct);
        var saved = await _context.SaveChangesAsync(ct);

        var encodedId = _hashids.EncodeLong(obj.Id);

        var responseObj = new SaveUrlResponse
        {
            Id = obj.Id,
            ShortUrl = encodedId,
            FullUrl = obj.FullUrl
        };

        var createdObj = new ObjectResult(responseObj)
        {
            StatusCode = StatusCodes.Status201Created
        };

        return saved > 0 ? createdObj : StatusCode(StatusCodes.Status500InternalServerError);
    }
}