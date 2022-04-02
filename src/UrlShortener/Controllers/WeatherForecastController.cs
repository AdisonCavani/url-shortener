using Microsoft.AspNetCore.Mvc;
using UrlShortener.Models;
using UrlShortener.Services;

namespace UrlShortener.Controllers;

[ApiController]
public class WeatherForecastController : ControllerBase
{
    public WeatherForecastController()
    {

    }

    [HttpGet("weather")]
    public IEnumerable<WeatherForecast> Get([FromServices]WeatherService weatherService)
    {
        return weatherService.GetWeatherForecast();
    }
}
