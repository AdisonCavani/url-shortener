namespace UrlShortener.Shared.Contracts.Requests;

public class SaveUserUrlRequest
{
    public string Url { get; set; } = default!;
}