using Ardalis.ApiEndpoints;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Swashbuckle.AspNetCore.Annotations;
using UrlShortener.Core.Contracts.V1;
using UrlShortener.WebApi.Models.Settings;

namespace UrlShortener.WebApi.Endpoints.Config;

public class Get : EndpointBaseSync.WithoutRequest.WithActionResult
{
    private readonly IOptions<AppSettings> _appSettings;
    private readonly IOptions<AuthSettings> _authSettings;

    public Get(IOptions<AppSettings> appSettings, IOptions<AuthSettings> authSettings)
    {
        _appSettings = appSettings;
        _authSettings = authSettings;
    }

    [Authorize]
    [HttpGet(ApiRoutes.Config.Get)]
    [SwaggerOperation(Tags = new[] { "Config Endpoint" })]
    public override ActionResult Handle()
    {
        return Ok(new
        {
            AppSettings = _appSettings.Value,
            AuthSettings = _authSettings.Value
        });
    }
}