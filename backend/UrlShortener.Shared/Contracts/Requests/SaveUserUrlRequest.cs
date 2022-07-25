using UrlShortener.Shared.Contracts.Dtos;

namespace UrlShortener.Shared.Contracts.Requests;

public class SaveUserUrlRequest
{
    public string Url { get; set; } = default!;

    public string? Title { get; set; }
    
    public List<TagDto>? Tags { get; set; }
}