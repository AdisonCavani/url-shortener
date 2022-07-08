namespace UrlShortener.WebApi.Extensions;

public static class Cache
{
    public static void AddCache(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddStackExchangeRedisCache(options =>
        {
            options.Configuration = configuration["AppSettings:RedisConnectionString"];
            // TODO: configure redis
        });
    }
}