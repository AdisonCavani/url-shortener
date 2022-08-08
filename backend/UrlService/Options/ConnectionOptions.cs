namespace UrlService.Options;

public class ConnectionOptions
{
    public const string SectionName = "ConnectionOptions";
    
    public string HashidsSalt { get; set; } = default!;
    
    public string RedisConnectionString { get; set; } = default!;
    
    public string PostgresConnectionString { get; set; } = default!;
}