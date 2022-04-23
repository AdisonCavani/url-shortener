using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using UrlShortener.Contracts.V1;
using UrlShortener.Data;

namespace UrlShortener.Controllers.V1;

[Authorize]
[ApiController]
[ApiVersion("1")]
public class ConfigController : ControllerBase
{
    [ProducesResponseType(200)]
    [HttpPost(ApiRoutes.Config.Get)]
    public IActionResult Config(
        [FromServices] IOptionsSnapshot<AuthSettings> authOptionsSnapshot,
        [FromServices] IOptionsSnapshot<AppSettings> optionsSnapshot)
    {
        return Ok(new
        {
            auth = authOptionsSnapshot.Value,
            config = optionsSnapshot.Value
        });
    }
}
