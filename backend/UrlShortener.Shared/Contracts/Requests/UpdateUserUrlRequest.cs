namespace UrlShortener.Shared.Contracts.Requests;

public class UpdateUserUrlRequest
{
    public long Id { get; set; }

    public string NewUrl { get; set; } = default!;
}