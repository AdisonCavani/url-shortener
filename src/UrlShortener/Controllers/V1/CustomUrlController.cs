using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace UrlShortener.Controllers.V1;

[Authorize]
[ApiController]
[ApiVersion("1")]
public class CustomUrlController : ControllerBase
{

}
