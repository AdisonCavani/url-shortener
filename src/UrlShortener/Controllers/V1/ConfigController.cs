using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using UrlShortener.Core.Contracts.V1;
using UrlShortener.Models.Settings;

namespace UrlShortener.Controllers.V1;

[Authorize]
[ApiController]
[ApiVersion("1")]
public class ConfigController : ControllerBase
{
    [ProducesResponseType(200)]
    [HttpGet(ApiRoutes.Config.Get)]
    public IActionResult Config(
        [FromServices] IOptionsSnapshot<AppSettings> appSettings,
        [FromServices] IOptionsSnapshot<AuthSettings> authSettings)
    {
        return Ok(new
        {
            AppSettings = appSettings.Value,
            AuthSettings = authSettings.Value
        });
    }
}
