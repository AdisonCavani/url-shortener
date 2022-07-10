using HashidsNet;
using StackExchange.Redis;
using UrlShortener.WebApi.HealthChecks;
using UrlShortener.WebApi.Models.App;
using UrlShortener.WebApi.Services;
using UrlShortener.WebApi.Services.Interfaces;

namespace UrlShortener.WebApi.Extensions;

public static class Services
{
    public static void RegisterServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddHealthChecks()
            .AddDbContextCheck<AppDbContext>()
            .AddCheck<RedisHealthCheck>("Redis");

#if RELEASE
        services.AddScoped<IEmailService, DevEmailService>();
#else
        services.AddScoped<IEmailService, EmailService>();
#endif
        services.AddScoped<EmailHandler>();
        services.AddScoped<JwtService>();

        services.AddSingleton<IHashids>(_ => new Hashids(configuration["AppSettings:HashidsSalt"], 7));

        services.AddScoped<UrlService>();
        services.AddScoped<IUrlService>(x =>
            new CachedUrlService(x.GetRequiredService<UrlService>(), x.GetRequiredService<IConnectionMultiplexer>()));
    }
}