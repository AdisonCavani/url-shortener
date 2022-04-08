using StackExchange.Redis;

namespace UrlShortener.Services;

public static class StartupServices
{
    public static IServiceCollection AddDependencyInjectionServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddSingleton<WeatherService>();
        services.AddSingleton<IConnectionMultiplexer>(x =>
            ConnectionMultiplexer.Connect(configuration.GetValue<string>("ConnectionStrings:RedisConnection")));
        services.AddSingleton<ICacheService, RedisCacheService>();

        return services;
    }
}
