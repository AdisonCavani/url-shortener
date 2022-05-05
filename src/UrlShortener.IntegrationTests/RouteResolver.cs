namespace UrlShortener.IntegrationTests;

public class RouteResolver
{
    private readonly string _baseUrl;
    private readonly string _apiVersion;

    public RouteResolver(string baseUrl, string apiVersion)
    {
        _baseUrl = baseUrl;
        _apiVersion = apiVersion;
    }

    // Api route resolver
    public string Get(string route) => _baseUrl + route.Replace("v{version:apiVersion}", _apiVersion);
}