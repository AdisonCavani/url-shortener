using HashidsNet;
using UrlShortener.Services;

namespace UrlShortener.Extensions;

public static class StartupServices
{
    public static IServiceCollection AddDependencyInjectionServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddSingleton<WeatherService>();

        services.AddSingleton<IHashids>(_ => new Hashids(configuration.GetValue<string>("Hashids:Salt"), configuration.GetValue<int>("Hashids:MinHashLenght")));

        // Redis cache
        services.AddStackExchangeRedisCache(options =>
        {
            options.Configuration = configuration.GetValue<string>("ConnectionStrings:RedisConnection");
        });
        services.AddDistributedMemoryCache();

        return services;
    }
}
