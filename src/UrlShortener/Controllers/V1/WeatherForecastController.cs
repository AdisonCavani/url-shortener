using Microsoft.AspNetCore.Mvc;
using UrlShortener.Contracts.V1;
using UrlShortener.Data;
using UrlShortener.Models;
using UrlShortener.Services;

namespace UrlShortener.Controllers.V1;

[ApiController]
[ApiVersion("1")]
public class WeatherForecastController : ControllerBase
{
    [HttpGet(ApiRoutes.Weather.Get)]
    public IEnumerable<WeatherForecast> Get([FromServices] WeatherService weatherService)
    {
        return weatherService.GetWeatherForecast();
    }

    [HttpPost(ApiRoutes.Weather.Save)]
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
