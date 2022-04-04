using Microsoft.AspNetCore.Mvc;
using UrlShortener.Data;
using UrlShortener.Models;
using UrlShortener.Services;

namespace UrlShortener.Controllers;

[ApiController]
public class WeatherForecastController : ControllerBase
{
    private readonly ILogger<WeatherForecastController> _logger;

    public WeatherForecastController(ILogger<WeatherForecastController> logger)
    {
        _logger = logger;
    }

    [HttpGet("weather")]
    public IEnumerable<WeatherForecast> Get([FromServices] WeatherService weatherService)
    {
        return weatherService.GetWeatherForecast();
    }

    [HttpPost("save")]
    public async Task<IActionResult> Set([FromServices] AppDbContext context, string name, string value)
    {
        try
        {
            context.Settings.Add(new SettingsDataModel
            {
                Name = name,
                Value = value
            });

            await context.SaveChangesAsync();

            return new StatusCodeResult(201);
        }

        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            return new StatusCodeResult(500);
        }
    }
}
