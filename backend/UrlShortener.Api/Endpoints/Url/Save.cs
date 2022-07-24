using Ardalis.ApiEndpoints;
using HashidsNet;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using UrlShortener.Api.Database;
using UrlShortener.Shared.Contracts;
using UrlShortener.Shared.Contracts.Requests;

namespace UrlShortener.Api.Endpoints.Url;

public class Save : EndpointBaseAsync.WithRequest<SaveUrlRequest>.WithActionResult
{
    private readonly AppDbContext _context;
    private readonly IHashids _hashids;

    public Save(AppDbContext context, IHashids hashids)
    {
        _context = context;
        _hashids = hashids;
    }

    [HttpPost(ApiRoutes.Url.Save)]
    [SwaggerOperation(Tags = new[] { "Url Endpoint" })]
    public override async Task<ActionResult> HandleAsync(SaveUrlRequest req, CancellationToken ct = default)
    {
        Database.Entities.Url obj = new()
        {
            FullUrl = req.Url
        };

        await _context.Urls.AddAsync(obj, ct);
        var saved = await _context.SaveChangesAsync(ct);

        var encodedId = _hashids.EncodeLong(obj.Id);

        var createdObj = new ObjectResult(encodedId)
        {
            StatusCode = StatusCodes.Status201Created
        };

        return saved > 0 ? createdObj : StatusCode(500);
    }
}