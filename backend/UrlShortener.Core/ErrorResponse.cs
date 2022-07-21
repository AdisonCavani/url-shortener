namespace UrlShortener.Core;

public class ErrorResponse
{
    public IEnumerable<string> Errors { get; set; } = Array.Empty<string>();
}
