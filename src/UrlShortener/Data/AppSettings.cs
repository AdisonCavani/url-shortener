namespace UrlShortener.Data;

public class AppSettings
{
    public string HashidsSalt { get; set; }

    public string RedisConnection { get; set; }

    public string SQLConnection { get; set; }
}
