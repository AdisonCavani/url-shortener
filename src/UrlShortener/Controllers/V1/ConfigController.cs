using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using UrlShortener.Contracts.V1;
using UrlShortener.Models.App;
using UrlShortener.Models.Responses;

namespace UrlShortener.Controllers.V1;

[Authorize]
[ApiController]
[ApiVersion("1")]
public class ConfigController : ControllerBase
{
    [ProducesResponseType(200)]
    [HttpPost(ApiRoutes.Config.Get)]
    public IActionResult Config(
        [FromServices] IOptionsSnapshot<AppSettings> appSettings,
        [FromServices] IOptionsSnapshot<AuthSettings> authSettings)
    {
        return Ok(new SettingsDto
        {
            AppSettings = appSettings.Value,
            AuthSettings = authSettings.Value
        });
    }
}
