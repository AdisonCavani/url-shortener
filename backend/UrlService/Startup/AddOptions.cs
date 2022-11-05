using UrlService.Options;

namespace UrlService.Startup;

public static class AddOptions
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
                    var rabbitMq = config.GetServiceUri("rabbitmq:amqp");
                    ArgumentNullException.ThrowIfNull(rabbitMq);
                    
                    opt.PostgresConnectionString = configuration.GetConnectionString("url-service-postgres");
                    opt.RedisConnectionString = configuration.GetConnectionString("url-service-redis");
                    opt.RabbitMqHost = rabbitMq.Host;
                    opt.RabbitMqPort = rabbitMq.Port;
                }

                config.Bind(ConnectionOptions.SectionName, opt);
            });
        
        services
            .AddOptions<LoggingOptions>()
            .Configure<IConfiguration>((opt, config) =>
            {
                // Replace specific connection strings in production
                if (env.IsProduction())
                {
                    opt.SeqConnectionString = config.GetConnectionString("seq");
                }

                config.Bind(LoggingOptions.SectionName, opt);
            });
    }
}