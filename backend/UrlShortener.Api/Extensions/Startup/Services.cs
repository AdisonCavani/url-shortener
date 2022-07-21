using HashidsNet;
using StackExchange.Redis;
using UrlShortener.Api.Models.App;
using UrlShortener.Api.Services.Endpoints;
using UrlShortener.Api.Services.Interfaces;

namespace UrlShortener.Api.Extensions.Startup;

public static class Services
{
    public static void RegisterServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddHealthChecks()
            .AddDbContextCheck<AppDbContext>()
            .AddCheck<RedisHealthCheck>("Redis");

        services.AddSingleton<IHashids>(_ => new Hashids(configuration["AppSettings:HashidsSalt"], 7));

        services.AddScoped<UrlService>();
        services.AddScoped<IUrlService>(x =>
            new CachedUrlService(x.GetRequiredService<UrlService>(), x.GetRequiredService<IConnectionMultiplexer>()));
    }
}