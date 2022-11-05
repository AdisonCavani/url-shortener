using UrlService.Options;

namespace UrlService.Startup;

public static class GetOptions
{
    public static AllOptions GetAllOptions(this WebApplicationBuilder builder)
    {
        var authOptions = new AuthOptions();
        var connectionOptions = new ConnectionOptions();
        var loggingOptions = new LoggingOptions();

        // Assign values from e.g: appsettings.json and userSecrets.json
        builder.Configuration.Bind(AuthOptions.SectionName, authOptions);
        builder.Configuration.Bind(ConnectionOptions.SectionName, connectionOptions);
        builder.Configuration.Bind(LoggingOptions.SectionName, loggingOptions);
        
        // Replace specific connection strings in production
        if (builder.Environment.IsProduction())
        {
            var rabbitMq = builder.Configuration.GetServiceUri("rabbitmq:amqp");
            ArgumentNullException.ThrowIfNull(rabbitMq);

            connectionOptions.PostgresConnectionString = builder.Configuration.GetConnectionString("url-service-postgres");
            connectionOptions.RedisConnectionString = builder.Configuration.GetConnectionString("url-service-redis");
            connectionOptions.RabbitMqHost = rabbitMq.Host;
            connectionOptions.RabbitMqPort = rabbitMq.Port;
            
            loggingOptions.SeqConnectionString = builder.Configuration.GetConnectionString("seq");
        }
        
        // Validate all settings
        authOptions.Validate();
        connectionOptions.Validate();
        loggingOptions.Validate(builder.Environment);

        // This will be used in Startup (ConfigureServices method) when, DI is not available (before Configure method)  
        return new()
        {
            AuthOptions = authOptions,
            ConnectionOptions = connectionOptions,
            LoggingOptions = loggingOptions
        };
    }
}