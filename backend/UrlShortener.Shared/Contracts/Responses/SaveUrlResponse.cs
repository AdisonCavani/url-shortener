namespace UrlShortener.Shared.Contracts.Responses;

public class SaveUrlResponse
{
    public long Id { get; set; }

    public string ShortUrl { get; set; } = default!;
    
    public string FullUrl { get; set; } = default!;
}