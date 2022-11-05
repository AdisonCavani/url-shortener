namespace ManagementService.Options;

public static class Validator
{
    public static void Validate(this ConnectionOptions options)
    {
        ArgumentNullException.ThrowIfNull(options.PostgresConnectionString);
        ArgumentNullException.ThrowIfNull(options.RabbitMqHost);
        ArgumentNullException.ThrowIfNull(options.RabbitMqPort);
    }
}