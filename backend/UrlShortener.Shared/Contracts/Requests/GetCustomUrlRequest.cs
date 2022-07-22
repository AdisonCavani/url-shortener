namespace UrlShortener.Shared.Contracts.Requests;

public class GetCustomUrlRequest
{
    public string Url { get; set; } = default!;
}