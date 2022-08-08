namespace UrlService.Options;

public class LoggingOptions
{
    public const string SectionName = "LoggingOptions";

    public string AppName { get; set; } = default!;
    
    public string ElasticConnectionString { get; set; } = default!;

    public string SeqConnectionString { get; set; } = default!;
}