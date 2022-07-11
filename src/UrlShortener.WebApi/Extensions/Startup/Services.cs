using HashidsNet;
using StackExchange.Redis;
using UrlShortener.WebApi.Models.App;
using UrlShortener.WebApi.Services.Auth;
using UrlShortener.WebApi.Services.Email;
using UrlShortener.WebApi.Services.Endpoints;
using UrlShortener.WebApi.Services.Interfaces;

namespace UrlShortener.WebApi.Extensions.Startup;

public static class Services
{
    public static void RegisterServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddHealthChecks()
            .AddDbContextCheck<AppDbContext>()
            .AddCheck<RedisHealthCheck>("Redis");

#if RELEASE
        // services.AddScoped<IEmailService, DevEmailService>();
        services.AddScoped<IEmailService, SmtpEmailService>();
        services.AddScoped<IEmailHandler, EmailHandler>();
#else
        services.AddScoped<IEmailService, SendgridEmailService>();
        services.AddScoped<IEmailHandler, SendgridEmailHandler>();
#endif

        services.AddScoped<JwtService>();

        services.AddSingleton<IHashids>(_ => new Hashids(configuration["AppSettings:HashidsSalt"], 7));

        services.AddScoped<UrlService>();
        services.AddScoped<IUrlService>(x =>
            new CachedUrlService(x.GetRequiredService<UrlService>(), x.GetRequiredService<IConnectionMultiplexer>()));
    }
}