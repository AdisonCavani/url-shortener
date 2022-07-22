namespace UrlShortener.Api.Configuration;

public class AppSettings
{
    public string HashidsSalt { get; set; } = default!;

    public string RedisConnectionString { get; set; } = default!;

    public string SqlConnectionString { get; set; } = default!;
}