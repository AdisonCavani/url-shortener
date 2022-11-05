using System.Net.Mime;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using UrlService.Contracts;
using UrlService.Contracts.Dtos;
using UrlService.Contracts.Responses;

namespace UrlService.Startup;

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
                    Checks = report.Entries.Select(x => new HealthCheckDto
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