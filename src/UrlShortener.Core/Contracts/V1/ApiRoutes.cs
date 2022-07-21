namespace UrlShortener.Core.Contracts.V1;

public static class ApiRoutes
{
    private const string _root = "api";
    private const string _version = "v{version:apiVersion}";
    private const string _base = $"{_root}/{_version}";

    public const string Health = "/api/health";
    
    public static class Config
    {
        private const string _endpoint = $"{_base}/config";

        public const string Get = $"{_endpoint}/get";
    }

    public static class CustomUrl
    {
        private const string _endpoint = $"{_base}/customurl";

        public const string Get = $"{_endpoint}/get";
        public const string Save = $"{_endpoint}/save";
        public const string Delete = $"{_endpoint}/delete";
    }

    public static class Url
    {
        private const string _endpoint = $"{_base}/url";

        public const string Get = $"{_endpoint}/get";
        public const string Save = $"{_endpoint}/save";
    }
}