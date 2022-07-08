using Ardalis.ApiEndpoints;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using UrlShortener.Core.Contracts.V1;
using UrlShortener.WebApi.Services.Interfaces;

namespace UrlShortener.WebApi.Endpoints.CustomUrl;

public class Get : EndpointBaseAsync.WithRequest<string>.WithActionResult
{
    private readonly IUrlService _urlService;

    public Get(IUrlService urlService)
    {
        _urlService = urlService;
    }
    
    [HttpGet(ApiRoutes.CustomUrl.Get)]
    [SwaggerOperation(Tags = new[] {"CustomUrl Endpoint"})]
    public override async Task<ActionResult> HandleAsync([FromQuery] string dto, CancellationToken ct = default)
    {
        var result = await _urlService.GetCustomUrlAsync(dto, ct);

        if (result is null)
            return NotFound();

        return Ok(result);
    }
}