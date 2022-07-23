namespace UrlShortener.Shared.Contracts.Responses;

public class GetAllUrlsResponse
{
    public IEnumerable<GetUrlResponse> Urls { get; set; } = Enumerable.Empty<GetUrlResponse>();
    
    public int Pages { get; set; }
    
    public int CurrentPage { get; set; }
}