using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using UrlShortener.Contracts.V2;
using UrlShortener.Data;

namespace UrlShortener.Controllers.V2;

[ApiController]
[ApiVersion("2")]
public class ConfigController : ControllerBase
{
    [Authorize]
    [HttpPost(ApiRoutes.Config.Get)]
    public IActionResult Config([FromServices] IOptionsSnapshot<AppSettings> optionsSnapshot)
    {
        return Ok(new
        {
            config = optionsSnapshot.Value
        });
    }
}
