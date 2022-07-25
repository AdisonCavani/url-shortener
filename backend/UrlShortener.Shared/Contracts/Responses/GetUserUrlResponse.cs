using UrlShortener.Shared.Contracts.Dtos;
using UrlShortener.Shared.Contracts.Requests;

namespace UrlShortener.Shared.Contracts.Responses;

public class GetUserUrlResponse
{
    public long Id { get; set; }

    public string ShortUrl { get; set; } = default!;
    
    public string FullUrl { get; set; } = default!;

    public UrlDetailsDto UrlDetails { get; set; } = default!;
}