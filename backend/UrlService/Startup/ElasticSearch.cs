using Serilog;
using Serilog.Formatting.Json;
using Serilog.Sinks.Elasticsearch;
using Serilog.Sinks.File;
using UrlService.Options;

namespace UrlService.Startup;

public static class ElasticSearch
{
    public static void UseElasticSearch(
        this LoggerConfiguration configuration,
        LoggingOptions loggingOptions,
        IWebHostEnvironment env)
    {
        configuration
            .WriteTo.Elasticsearch(
                new ElasticsearchSinkOptions(new Uri(loggingOptions.ElasticConnectionString))
                {
                    IndexFormat =
                        $"{loggingOptions.AppName}-logs-{env.EnvironmentName?.ToLower().Replace('.', '-')}-{DateTime.UtcNow:yyyy-MM}",
                    AutoRegisterTemplate = true,
                    NumberOfShards = 2,
                    NumberOfReplicas = 1,

                    // Handling errors
                    FailureCallback = e => Log.Error($"ES was unable to submit event {e.MessageTemplate}"),
                    EmitEventFailure = EmitEventFailureHandling.WriteToSelfLog |
                                       EmitEventFailureHandling.WriteToFailureSink |
                                       EmitEventFailureHandling.RaiseCallback,
                    FailureSink =
                        new FileSink(Path.Combine(AppContext.BaseDirectory, "logs", "ES-failures.txt"),
                            new JsonFormatter(), null)
                });
    }
}