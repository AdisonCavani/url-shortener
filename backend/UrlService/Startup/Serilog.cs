using Serilog;
using UrlService.Options;

namespace UrlService.Startup;

public static class Serilog
{
    public static void AddSerilog(this ConfigureWebHostBuilder builder, IWebHostEnvironment env, LoggingOptions loggingOptions)
    {
        builder.UseSerilog((context, configuration) =>
        {
            configuration
                .Enrich.FromLogContext()
                .Enrich.WithMachineName()
                .Enrich.WithProperty("Environment", context.HostingEnvironment.EnvironmentName)
                .ReadFrom.Configuration(context.Configuration);

            if (env.IsProduction())
                configuration.WriteTo.Seq(loggingOptions.SeqConnectionString);
                // configuration.UseElasticSearch(loggingOptions, context.HostingEnvironment);
            
            else
                configuration
                    .WriteTo.Console()
                    .WriteTo.File(Path.Combine(AppContext.BaseDirectory, "logs", "log.txt"), rollingInterval: RollingInterval.Day);
        });
    }
}