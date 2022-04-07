using Swashbuckle.AspNetCore.Filters;
using UrlShortener.Models;

namespace UrlShortener.SwaggerExamples.Responses.V2;

public class WeatherForecastResponseExample : IExamplesProvider<IEnumerable<WeatherForecast>>
{
    public IEnumerable<WeatherForecast> GetExamples()
    {
        return new List<WeatherForecast>
        {
            new WeatherForecast
            {
                Date = DateTime.Now,
                TemperatureC = 48,
                Summary = "Warm"
            },
            new WeatherForecast
            {
                Date= DateTime.Now,
                TemperatureC = 13,
                Summary = "Chilly"
            }
        };
    }
}
