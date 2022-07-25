using UrlShortener.Shared.Contracts.Dtos;

namespace UrlShortener.Shared.Contracts.Requests;

public class UpdateUserUrlRequest
{
    public long Id { get; set; }

    public string Url { get; set; } = default!;

    public string? Title { get; set; }
    
    public List<TagDto>? Tags { get; set; }
}