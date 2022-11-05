using ManagementService.Options;

namespace ManagementService.Startup;

public static class AddOptions
{
    public static void ConfigureOptions(
        this IServiceCollection services,
        IConfiguration configuration,
        IWebHostEnvironment env)
    {
        services
            .AddOptions<ConnectionOptions>()
            .Configure<IConfiguration>((opt, config) =>
            {
                // Replace specific connection strings in production
                if (env.IsProduction())
                {
                    var rabbitMq = config.GetServiceUri("rabbitmq:amqp");
                    ArgumentNullException.ThrowIfNull(rabbitMq);
                    
                    opt.PostgresConnectionString = configuration.GetConnectionString("management-service-postgres");
                    opt.RabbitMqHost = rabbitMq.Host;
                    opt.RabbitMqPort = rabbitMq.Port;
                }

                config.Bind(ConnectionOptions.SectionName, opt);
            });
    }
}