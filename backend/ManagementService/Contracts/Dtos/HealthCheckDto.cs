namespace ManagementService.Contracts.Dtos;

public class HealthCheckDto
{
    public string Status { get; set; } = default!;

    public string Component { get; set; } = default!;

    public string? Description { get; set; }
}