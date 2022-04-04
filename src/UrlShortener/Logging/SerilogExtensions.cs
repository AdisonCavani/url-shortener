using Serilog;
using Serilog.Sinks.Elasticsearch;

namespace UrlShortener;

public static class SerilogExtensions
{
    /// <summary>
    /// An extension method to configure Serilog logger
    /// </summary>
    /// <param name="builder"></param>
    /// <returns></returns>
    public static IHostBuilder AddSerilog(this IHostBuilder builder)
    {
        builder.UseSerilog((context, configuration) =>
        {
            configuration.Enrich.FromLogContext()
                .Enrich.WithMachineName()
#if DEBUG
                .WriteTo.Console()
                .WriteTo.File(Path.Combine(AppContext.BaseDirectory, "logs", "log.txt"), rollingInterval: RollingInterval.Day)
#endif
                .WriteTo.Elasticsearch(new ElasticsearchSinkOptions(new Uri(context.Configuration["ElasticConfiguration:Uri"]))
                {
                    IndexFormat = $"{context.Configuration["AppName"]}-logs-{context.HostingEnvironment.EnvironmentName?.ToLower().Replace('.', '-')}-{DateTime.UtcNow:yyyy-MM}",
                    AutoRegisterTemplate = true,
                    NumberOfShards = 2,
                    NumberOfReplicas = 1,

                    // Handling errors
                    /*
                    FailureCallback = e => Log.Error("ES was unable to submit event " + e.MessageTemplate),
                    EmitEventFailure = EmitEventFailureHandling.WriteToSelfLog |
                                       EmitEventFailureHandling.WriteToFailureSink |
                                       EmitEventFailureHandling.RaiseCallback,
                    FailureSink = new FileSink(Path.Combine(AppContext.BaseDirectory, "logs", "ESfailures.txt"), new JsonFormatter(), null)
                    */

                })
                .Enrich.WithProperty("Environment", context.HostingEnvironment.EnvironmentName)
                .ReadFrom.Configuration(context.Configuration);
        });

        return builder;
    }
}
