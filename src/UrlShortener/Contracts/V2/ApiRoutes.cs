namespace UrlShortener.Contracts.V2;

public static class ApiRoutes
{
    private const string Root = "api";
    private const string Version = "v{version:apiVersion}";
    private const string Base = Root + "/" + Version + "/";

    public static class Weather
    {
        private const string Endpoint = "weather";

        public const string Get = Base + Endpoint + "/get";

        public const string Save = Base + Endpoint + "/post";
    }
}
