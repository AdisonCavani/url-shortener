using Microsoft.AspNetCore.Mvc;
using UrlShortener.Data;
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
    public IEnumerable<WeatherForecast> Get([FromServices] WeatherService weatherService)
    {
        return weatherService.GetWeatherForecast();
    }

    [HttpPost("save/{name}/{value}")]
    public async Task<ActionResult> Set([FromServices] AppDbContext context, string name, string value)
    {
        context.Settings.Add(new SettingsDataModel
        {
            Name = name,
            Value = value
        });

        return Ok(await context.SaveChangesAsync());
    }
}
