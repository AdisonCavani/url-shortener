using Microsoft.AspNetCore.Mvc;
using UrlShortener.Contracts.V1;
using UrlShortener.Models;
using UrlShortener.Services;

namespace UrlShortener.Controllers.V1;

[ApiController]
[ApiVersion("1")]
public class WeatherForecastController : ControllerBase
{
    /// <summary>
    /// Gets a weather forecast
    /// </summary>
    /// <response code="200">Success</response>
    [HttpGet(ApiRoutes.Weather.Get)]
    [ProducesResponseType(typeof(IEnumerable<WeatherForecast>), 200)]
    public IEnumerable<WeatherForecast> Get([FromServices] WeatherService weatherService)
    {
        return weatherService.GetWeatherForecast();
    }
}
