using Microsoft.AspNetCore.Mvc;
using UrlShortener.Contracts.V2;
using UrlShortener.Data;
using UrlShortener.Models;
using UrlShortener.Services;

namespace UrlShortener.Controllers.V2;

[ApiController]
[ApiVersion("2")]
public class WeatherForecastController : ControllerBase
{
    [HttpGet(ApiRoutes.Weather.Get)]
    public IEnumerable<WeatherForecast> Get([FromServices] WeatherService weatherService)
    {
        return weatherService.GetWeatherForecast();
    }

    [HttpPut(ApiRoutes.Weather.Save)]
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
