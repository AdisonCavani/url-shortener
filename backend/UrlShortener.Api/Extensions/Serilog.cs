using Serilog;

namespace UrlShortener.Api.Extensions;

public static class Serilog
{
    public static IHostBuilder AddSerilog(this IHostBuilder builder)
    {
        builder.UseSerilog((context, configuration) =>
        {
            configuration
                .Enrich.FromLogContext()
                .Enrich.WithMachineName()
#if DEBUG
                .WriteTo.Console()
                .WriteTo.File(Path.Combine(AppContext.BaseDirectory, "logs", "log.txt"),
                    rollingInterval: RollingInterval.Day)
#endif
#if RELEASE
                .UseElasticSearch(context)
#endif
                .Enrich.WithProperty("Environment", context.HostingEnvironment.EnvironmentName)
                .ReadFrom.Configuration(context.Configuration);
        });

        return builder;
    }
}