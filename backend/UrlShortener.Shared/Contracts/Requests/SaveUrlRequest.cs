namespace UrlShortener.Shared.Contracts.Requests;

public class SaveUrlRequest
{
    public string Url { get; set; } = default!;
}