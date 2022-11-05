using System.Net.Mime;
using ManagementService.Contracts;
using ManagementService.Contracts.Dtos;
using ManagementService.Contracts.Responses;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;

namespace ManagementService.Startup;

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