namespace UrlService.Options;

public class AllOptions
{
    public AuthOptions AuthOptions { get; set; } = default!;

    public ConnectionOptions ConnectionOptions { get; set; } = default!;

    public LoggingOptions LoggingOptions { get; set; } = default!;
}