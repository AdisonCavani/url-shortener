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

    /// <summary>
    /// Saves a new <see cref="SettingsDataModel"/> to SQL database
    /// </summary>
    /// <param name="name" example="Background">The settings name</param>
    /// <param name="value" example="Red">The settings value</param>
    /// <response code="201">Successfully saved</response>
    [HttpPut(ApiRoutes.Weather.Save)]
    [ProducesResponseType(201)]
    public async Task<IActionResult> Save([FromServices] AppDbContext context, string name, string value)
    {
        await context.Settings.AddAsync(new SettingsDataModel
        {
            Name = name,
            Value = value
        });

        await context.SaveChangesAsync();

        return new StatusCodeResult(201);
    }
}
