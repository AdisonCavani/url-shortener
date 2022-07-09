using Ardalis.ApiEndpoints;
using HashidsNet;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using UrlShortener.Core.Contracts.V1;
using UrlShortener.WebApi.Models.App;

namespace UrlShortener.WebApi.Endpoints.Url;

public class Save : EndpointBaseAsync.WithRequest<string>.WithActionResult
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
    public override async Task<ActionResult> HandleAsync([FromQuery] string dto, CancellationToken ct = default)
    {
        Models.Entities.Url obj = new()
        {
            FullUrl = dto
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