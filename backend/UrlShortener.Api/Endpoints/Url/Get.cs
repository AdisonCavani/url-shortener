using Ardalis.ApiEndpoints;
using HashidsNet;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using UrlShortener.Api.Services.Interfaces;
using UrlShortener.Shared.Contracts;
using UrlShortener.Shared.Contracts.Requests;

namespace UrlShortener.Api.Endpoints.Url;

public class Get : EndpointBaseAsync.WithRequest<GetUrlRequest>.WithActionResult<string>
{
    private readonly IHashids _hashids;
    private readonly IUrlService _urlService;

    public Get(IHashids hashids, IUrlService urlService)
    {
        _hashids = hashids;
        _urlService = urlService;
    }
    
    [HttpGet(ApiRoutes.Url.Get)]
    [SwaggerOperation(Tags = new[] { "Url Endpoint" })]
    public override async Task<ActionResult<string>> HandleAsync(GetUrlRequest req, CancellationToken ct = default)
    {
        var rawId = _hashids.Decode(req.Id);

        if (rawId.Length == 0)
            return BadRequest();

        var result = await _urlService.GetUrlByIdAsync(rawId[0], ct);

        if (result is null)
            return NotFound();

        return Ok(result);
    }
}
