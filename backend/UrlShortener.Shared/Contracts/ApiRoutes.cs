namespace UrlShortener.Shared.Contracts;

public static class ApiRoutes
{
    private const string Root = "api";
    private const string Version = "v{version:apiVersion}";
    private const string Base = $"{Root}/{Version}";

    public const string Health = "/api/health";
    
    public static class Config
    {
        private const string Endpoint = $"{Base}/config";

        public const string Get = $"{Endpoint}/get";
    }

    public static class CustomUrl
    {
        private const string Endpoint = $"{Base}/customurl";

        public const string Get = $"{Endpoint}/get";
        public const string Save = $"{Endpoint}/save";
        public const string Delete = $"{Endpoint}/delete";
    }

    public static class Url
    {
        private const string Endpoint = $"{Base}/url";

        public const string Get = $"{Endpoint}/get";
        public const string Save = $"{Endpoint}/save";
    }
}