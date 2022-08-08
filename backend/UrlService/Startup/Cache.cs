using StackExchange.Redis;
using UrlService.Options;

namespace UrlService.Startup;

public static class Cache
{
    public static void AddCache(this IServiceCollection services, ConnectionOptions connectionOptions)
    {
        services.AddSingleton<IConnectionMultiplexer>(_ =>
            ConnectionMultiplexer.Connect(connectionOptions.RedisConnectionString));
    }
}