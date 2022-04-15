using HashidsNet;
using UrlShortener.Services;

namespace UrlShortener.Extensions;

public static class StartupServices
{
    public static IServiceCollection AddDependencyInjectionServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddSingleton<WeatherService>();

        services.AddSingleton<IHashids>(_ => new Hashids(configuration["AppSettings:HashidsSalt"], 7));

        // Redis cache
        services.AddStackExchangeRedisCache(options =>
        {
            options.Configuration = configuration["AppSettings:RedisConnection"];
        });
        services.AddDistributedMemoryCache();

        return services;
    }
}
