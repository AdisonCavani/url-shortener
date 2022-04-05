﻿using Serilog;
using Serilog.Sinks.Elasticsearch;

namespace UrlShortener.Logging;

public static class ElasticSearchExtensions
{
    public static LoggerConfiguration UseElasticSearch(this LoggerConfiguration configuration, HostBuilderContext context)
    {
        configuration
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
            });

        return configuration;
    }
}
