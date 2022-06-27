using UrlShortener.Models.App;

namespace UrlShortener.Extensions;

public static class Settings
{
    public static void ConfigureSettings(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<AppSettings>(configuration.GetSection(nameof(AppSettings)));
        services.Configure<AuthSettings>(configuration.GetSection(nameof(AuthSettings)));
    }
}