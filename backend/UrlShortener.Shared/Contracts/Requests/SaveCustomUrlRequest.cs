namespace UrlShortener.Shared.Contracts.Requests;

public class SaveCustomUrlRequest
{
    public string ShortUrl { get; set; } = default!;

    public string FullUrl { get; set; } = default!;
}