namespace UrlShortener.Shared.Contracts.Responses;

public class HealthCheck
{
    public string Status { get; set; } = default!;

    public string Component { get; set; } = default!;

    public string? Description { get; set; }
}

public class HealthCheckResponse
{
    public string Status { get; set; } = default!;

    public IEnumerable<HealthCheck> Checks { get; set; } = Enumerable.Empty<HealthCheck>();

    public TimeSpan Duration { get; set; }
}