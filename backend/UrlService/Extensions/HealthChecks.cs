using System.Net.Mime;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using UrlShortener.Shared.Contracts;
using UrlShortener.Shared.Contracts.Responses;

namespace UrlService.Extensions;

public static class HealthChecks
{
    public static void UseHealthChecksEndpoint(this IApplicationBuilder builder)
    {
        builder.UseHealthChecks(ApiRoutes.Health, new HealthCheckOptions
        {
            ResponseWriter = async (httpContext, report) =>
            {
                httpContext.Response.ContentType = MediaTypeNames.Application.Json;

                var response = new HealthCheckResponse
                {
                    Status = report.Status.ToString(),
                    Checks = report.Entries.Select(x => new HealthCheck
                    {
                        Component = x.Key,
                        Status = x.Value.Status.ToString(),
                        Description = x.Value.Description
                    }),
                    Duration = report.TotalDuration
                };

                await httpContext.Response.WriteAsJsonAsync(response);
            }
        });
    }
}