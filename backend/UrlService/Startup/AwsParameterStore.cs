namespace UrlService.Startup;

public static class AwsParameterStore
{
    public static void AddAwsParameterStore(this ConfigureWebHostBuilder builder)
    {
        builder.ConfigureAppConfiguration(confBuilder =>
        {
            confBuilder.AddSystemsManager(config =>
            {
                var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")?.ToLower() ?? "development";

                // Parameter Store prefix to pull configuration data from.
                config.Path = $"/url-service/{environment}";

                // Reload configuration data every 15 minutes.
                config.ReloadAfter = TimeSpan.FromMinutes(15);

                // AWSOptions credentials are configured using AWS CLI
            });
        });
    }
}
