namespace UrlShortener.Api.Extensions.Startup;

public static class AwsParameterStore
{
    public static IHostBuilder AddAwsParameterStore(this IHostBuilder builder)
    {
        builder.ConfigureAppConfiguration(confBuilder =>
        {
            confBuilder.AddSystemsManager(config =>
            {
                var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")?.ToLower(); // FIX: this will not work in test env

                // Parameter Store prefix to pull configuration data from.
                config.Path = $"/{environment ?? "development"}/url-shortener";

                // Reload configuration data every 15 minutes.
                config.ReloadAfter = TimeSpan.FromMinutes(15);

                // AWSOptions credentials are configured using AWS CLI
            });
        });

        return builder;
    }
}
