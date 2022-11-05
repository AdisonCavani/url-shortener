using UrlService.Contracts.Dtos;

namespace UrlService.Contracts.Responses;

public class HealthCheckResponse
{
    public string Status { get; set; } = default!;

    public IEnumerable<HealthCheckDto> Checks { get; set; } = Enumerable.Empty<HealthCheckDto>();

    public TimeSpan Duration { get; set; }
}