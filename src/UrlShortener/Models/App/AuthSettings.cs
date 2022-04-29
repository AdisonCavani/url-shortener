namespace UrlShortener.Models.App;

public class AuthSettings
{
    public string JwtKey { get; set; }

    public int JwtExpireMinutes { get; set; }

    public string JwtIssuer { get; set; }
}
