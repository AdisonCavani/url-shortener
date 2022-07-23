namespace UrlShortener.Shared.Contracts.Requests;

public class UpdateUrlRequest
{
    public long Id { get; set; }

    public string NewUrl { get; set; } = default!;
}