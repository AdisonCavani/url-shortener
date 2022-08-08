using Ardalis.ApiEndpoints;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Swashbuckle.AspNetCore.Annotations;
using UrlService.Options;
using UrlShortener.Shared.Contracts;

namespace UrlService.Endpoints.Config;

public class Get : EndpointBaseSync.WithoutRequest.WithActionResult
{
    private readonly IOptions<AuthOptions> _authOptions;
    private readonly IOptions<ConnectionOptions> _connectionOptions;

    public Get(IOptions<AuthOptions> authOptions, IOptions<ConnectionOptions> connectionOptions)
    {
        _authOptions = authOptions;
        _connectionOptions = connectionOptions;
    }

    [Authorize]
    [HttpGet(ApiRoutes.Config.Get)]
    [SwaggerOperation(Tags = new[] {"Config Endpoint"})]
    public override ActionResult Handle()
    {
        return Ok(new
        {
            AuthOptions = _authOptions.Value,
            ConnectionOptions = _connectionOptions.Value
        });
    }
}