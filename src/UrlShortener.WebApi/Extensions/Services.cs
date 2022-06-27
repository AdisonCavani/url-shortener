using HashidsNet;
using UrlShortener.WebApi.Services;
using UrlShortener.WebApi.Services.Interfaces;

namespace UrlShortener.WebApi.Extensions;

public static class Services
{
    public static void RegisterServices(this IServiceCollection services, IConfiguration configuration)
    {
#if RELEASE
        services.AddScoped<IEmailService, DevEmailService>();
#else
        services.AddScoped<IEmailService, EmailService>();
#endif
        services.AddScoped<EmailHandler>();
        services.AddScoped<JwtService>();

        services.AddSingleton<IHashids>(_ => new Hashids(configuration["AppSettings:HashidsSalt"], 7));
    }
}