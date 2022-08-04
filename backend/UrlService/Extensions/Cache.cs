using StackExchange.Redis;

namespace UrlService.Extensions;

public static class Cache
{
    public static void AddCache(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddSingleton<IConnectionMultiplexer>(_ =>
            ConnectionMultiplexer.Connect(configuration["AppSettings:RedisConnectionString"]));
    }
}