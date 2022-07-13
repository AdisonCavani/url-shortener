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
    private readonly IOptions<SendGridSettings> _sendGridSettings;
    private readonly IOptions<SmtpSettings> _smtpSettings;

    public Get(
        IOptions<AppSettings> appSettings,
        IOptions<AuthSettings> authSettings,
        IOptions<SendGridSettings> sendGridSettings,
        IOptions<SmtpSettings> smtpSettings)
    {
        _appSettings = appSettings;
        _authSettings = authSettings;
        _sendGridSettings = sendGridSettings;
        _smtpSettings = smtpSettings;
    }

    [Authorize]
    [HttpGet(ApiRoutes.Config.Get)]
    [SwaggerOperation(Tags = new[] { "Config Endpoint" })]
    public override ActionResult Handle()
    {
        return Ok(new
        {
            AppSettings = _appSettings.Value,
            AuthSettings = _authSettings.Value,
            SendGridSettings = _sendGridSettings.Value,
            SmtpSettings = _smtpSettings.Value,
        });
    }
}