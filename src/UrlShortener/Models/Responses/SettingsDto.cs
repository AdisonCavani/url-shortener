using UrlShortener.Models.App;

namespace UrlShortener.Models.Responses;

public class SettingsDto
{
    public AppSettings AppSettings { get; init; }

    public AuthSettings AuthSettings { get; init; }
}
