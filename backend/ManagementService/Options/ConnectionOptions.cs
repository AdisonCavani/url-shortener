namespace ManagementService.Options;

public class ConnectionOptions
{
    public const string SectionName = "ConnectionOptions";

    public string PostgresConnectionString { get; set; } = default!;
    
    public string RabbitMqHost { get; set; } = default!;
    
    public int RabbitMqPort { get; set; }
}