using UrlService.Options;

namespace UrlService.Startup;

public static class Options
{
    public static void ConfigureOptions(
        this IServiceCollection services,
        IConfiguration configuration,
        IWebHostEnvironment env)
    {
        services
            .AddOptions<AuthOptions>()
            .Configure<IConfiguration>((opt, config) => config.Bind(AuthOptions.SectionName, opt));

        services
            .AddOptions<ConnectionOptions>()
            .Configure<IConfiguration>((opt, config) =>
            {
                // Replace specific connection strings in production
                if (env.IsProduction())
                {
                    opt.PostgresConnectionString = configuration.GetConnectionString("url-service-postgres");
                    opt.RedisConnectionString = configuration.GetConnectionString("url-service-redis");
                }

                config.Bind(ConnectionOptions.SectionName);
            });
    }
}