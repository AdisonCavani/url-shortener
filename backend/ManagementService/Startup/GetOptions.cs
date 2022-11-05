using ManagementService.Options;

namespace ManagementService.Startup;

public static class GetOptions
{
    public static AllOptions GetAllOptions(this WebApplicationBuilder builder)
    {
        var connectionOptions = new ConnectionOptions();

        // Assign values from e.g: appsettings.json and userSecrets.json
        builder.Configuration.Bind(ConnectionOptions.SectionName, connectionOptions);
        
        // Replace specific connection strings in production
        if (builder.Environment.IsProduction())
        {
            var rabbitMq = builder.Configuration.GetServiceUri("rabbitmq:amqp");
            ArgumentNullException.ThrowIfNull(rabbitMq);
            
            connectionOptions.PostgresConnectionString = builder.Configuration.GetConnectionString("management-service-postgres");
            connectionOptions.RabbitMqHost = rabbitMq.Host;
            connectionOptions.RabbitMqPort = rabbitMq.Port;
        }
        
        // Validate all settings
        connectionOptions.Validate();

        // This will be used in Startup (ConfigureServices method) when, DI is not available (before Configure method)  
        return new()
        {
            ConnectionOptions = connectionOptions
        };
    }
}