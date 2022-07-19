using HashidsNet;
using StackExchange.Redis;
using UrlShortener.WebApi.Models.App;
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

        services.AddSingleton<IHashids>(_ => new Hashids(configuration["AppSettings:HashidsSalt"], 7));

        services.AddScoped<UrlService>();
        services.AddScoped<IUrlService>(x =>
            new CachedUrlService(x.GetRequiredService<UrlService>(), x.GetRequiredService<IConnectionMultiplexer>()));
    }
}