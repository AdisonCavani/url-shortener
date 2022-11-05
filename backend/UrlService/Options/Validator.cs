namespace UrlService.Options;

public static class Validator
{
    public static void Validate(this AuthOptions options)
    {
        ArgumentNullException.ThrowIfNull(options.Audience);
        ArgumentNullException.ThrowIfNull(options.Issuer);
    }

    public static void Validate(this ConnectionOptions options)
    {
        ArgumentNullException.ThrowIfNull(options.HashidsSalt);
        ArgumentNullException.ThrowIfNull(options.RedisConnectionString);
        ArgumentNullException.ThrowIfNull(options.PostgresConnectionString);
        ArgumentNullException.ThrowIfNull(options.RabbitMqHost);
        ArgumentNullException.ThrowIfNull(options.RabbitMqPort);
    }

    public static void Validate(this LoggingOptions options, IWebHostEnvironment env)
    {
        if (env.IsProduction())
        {
            // ArgumentNullException.ThrowIfNull(options.AppName);
            // ArgumentNullException.ThrowIfNull(options.ElasticConnectionString);
            ArgumentNullException.ThrowIfNull(options.SeqConnectionString);
        }
    }
}