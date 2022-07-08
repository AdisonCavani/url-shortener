namespace UrlShortener.Core.Models.Responses;

public class HealthCheck
{
    public string Status { get; init; }
    
    public string Component { get; init; }
    
    public string? Description { get; set; }
}

public class HealthCheckResponse
{
    public string Status { get; init; }
    
    public IEnumerable<HealthCheck> Checks { get; init; }
    
    public TimeSpan Duration { get; init; }
}