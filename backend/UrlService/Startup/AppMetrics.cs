using App.Metrics.AspNetCore;
using App.Metrics.Formatters.Prometheus;

namespace UrlService.Startup;

public static class AppMetrics
{
    public static void AddAppMetrics(this ConfigureWebHostBuilder builder)
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
    }
}