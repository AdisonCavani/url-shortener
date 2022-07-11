using UrlShortener.WebApi.Models.Settings;

namespace UrlShortener.WebApi.Extensions.Startup;

public static class Settings
{
    public static void ConfigureSettings(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<AppSettings>(configuration.GetSection(nameof(AppSettings)));
        services.Configure<AuthSettings>(configuration.GetSection(nameof(AuthSettings)));
        services.Configure<SmtpSettings>(configuration.GetSection(nameof(SmtpSettings)));
        services.Configure<SendGridSettings>(configuration.GetSection(nameof(SendGridSettings)));
    }
}