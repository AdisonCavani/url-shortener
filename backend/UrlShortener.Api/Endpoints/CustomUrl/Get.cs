using Ardalis.ApiEndpoints;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using UrlShortener.Api.Services;
using UrlShortener.Shared.Contracts;
using UrlShortener.Shared.Contracts.Requests;

namespace UrlShortener.Api.Endpoints.CustomUrl;

public class Get : EndpointBaseAsync.WithRequest<GetCustomUrlRequest>.WithActionResult<string>
{
    private readonly IUrlService _urlService;

    public Get(IUrlService urlService)
    {
        _urlService = urlService;
    }

    [HttpGet(ApiRoutes.CustomUrl.Get)]
    [SwaggerOperation(Tags = new[] { "CustomUrl Endpoint" })]
    public override async Task<ActionResult<string>> HandleAsync(GetCustomUrlRequest req, CancellationToken ct = default)
    {
        var result = await _urlService.GetCustomUrlAsync(req.Url, ct);

        if (result is null)
            return NotFound();

        return Ok(result);
    }
}