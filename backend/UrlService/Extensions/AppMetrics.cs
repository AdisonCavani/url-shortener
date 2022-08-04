using App.Metrics.AspNetCore;
using App.Metrics.Formatters.Prometheus;

namespace UrlService.Extensions;

public static class AppMetrics
{
    public static IHostBuilder AddAppMetrics(this IHostBuilder builder)
    {
        builder.UseMetrics(options =>
        {
            options.EndpointOptions = endpointOptions =>
            {
                endpointOptions.MetricsEndpointOutputFormatter = new MetricsPrometheusTextOutputFormatter();
                endpointOptions.EnvironmentInfoEndpointEnabled = false;
                endpointOptions.MetricsTextEndpointEnabled = false;
            };
        });

        return builder;
    }
}