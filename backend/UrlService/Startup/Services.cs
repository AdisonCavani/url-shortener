using HashidsNet;
using StackExchange.Redis;
using UrlService.Database;
using UrlService.Options;
using UrlService.Services;
using UrlService.Services.Interfaces;

namespace UrlService.Startup;

public static class Services
{
    public static void RegisterServices(this IServiceCollection services, ConnectionOptions connectionOptions)
    {
        services.AddHealthChecks()
            .AddDbContextCheck<AppDbContext>()
            .AddCheck<RedisHealthCheck>("Redis");

        services.AddSingleton<IHashids>(_ => new Hashids(connectionOptions.HashidsSalt, 7));

        services.AddScoped<UrlService.Services.UrlService>();
        services.AddScoped<IUrlService>(x =>
            new CachedUrlService(x.GetRequiredService<UrlService.Services.UrlService>(), x.GetRequiredService<IConnectionMultiplexer>()));
    }
}