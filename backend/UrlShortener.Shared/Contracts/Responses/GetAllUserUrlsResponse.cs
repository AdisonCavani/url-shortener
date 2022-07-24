namespace UrlShortener.Shared.Contracts.Responses;

public class GetAllUserUrlsResponse
{
    public IEnumerable<GetUserUrlResponse> Urls { get; set; } = Enumerable.Empty<GetUserUrlResponse>();
    
    public int Pages { get; set; }
    
    public int CurrentPage { get; set; }
}