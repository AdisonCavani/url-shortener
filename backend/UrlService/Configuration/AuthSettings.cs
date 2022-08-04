namespace UrlService.Configuration;

public class AuthSettings
{
    public string Audience { get; set; } = default!;

    public string Issuer { get; set; } = default!;
}