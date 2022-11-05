using HashidsNet;
using StackExchange.Redis;
using UrlService.Database;
using UrlService.Options;
using UrlService.Services;

namespace UrlService.Startup;

public static class AddServices
{
    public static void RegisterServices(this IServiceCollection services, ConnectionOptions connectionOptions)
    {
        services.AddHealthChecks()
            .AddDbContextCheck<AppDbContext>()
            .AddCheck<RedisHealthCheck>("Redis");

        services.AddSingleton<IHashids>(_ => new Hashids(connectionOptions.HashidsSalt, 7));

        services.AddSingleton<IMessageBusPublisher, MessageBusPublisher>();
        services.AddScoped<UrlRepository>();
        services.AddScoped<IUrlRepository>(x =>
            new CachedUrlRepository(x.GetRequiredService<UrlRepository>(), x.GetRequiredService<IConnectionMultiplexer>()));
    }
}