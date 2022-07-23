namespace UrlShortener.Shared.Contracts.Responses;

public class GetUrlResponse
{
    public long Id { get; set; }

    public string ShortUrl { get; set; } = default!;
    
    public string FullUrl { get; set; } = default!;
}