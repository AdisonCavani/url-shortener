using Microsoft.AspNetCore.Mvc;
using UrlShortener.Data;
using UrlShortener.Models;
using UrlShortener.Services;

namespace UrlShortener.Controllers.V1;

[ApiController]
[ApiVersion("1")]
[Route("api/v{version:apiVersion}/weather")]
public class WeatherForecastController : ControllerBase
{
    [HttpGet("get")]
    public IEnumerable<WeatherForecast> Get([FromServices] WeatherService weatherService)
    {
        return weatherService.GetWeatherForecast();
    }

    [HttpPost("save")]
    public async Task<IActionResult> Save([FromServices] AppDbContext context, string name, string value)
    {
        context.Settings.Add(new SettingsDataModel
        {
            Name = name,
            Value = value
        });

        await context.SaveChangesAsync();

        return new StatusCodeResult(201);
    }
}
