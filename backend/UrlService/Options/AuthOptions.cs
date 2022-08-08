namespace UrlService.Options;

public class AuthOptions
{
    public const string SectionName = "AuthOptions";
    
    public string Audience { get; set; } = default!;

    public string Issuer { get; set; } = default!;
}