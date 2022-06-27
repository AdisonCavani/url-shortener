namespace UrlShortener.WebApi.Models.Settings;

public class AppSettings
{
    public string HashidsSalt { get; set; }

    public string RedisConnectionString { get; set; }

    public string SqlConnectionString { get; set; }
}
