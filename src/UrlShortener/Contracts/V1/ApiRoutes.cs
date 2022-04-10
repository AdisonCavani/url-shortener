namespace UrlShortener.Contracts.V1;

public static class ApiRoutes
{
    private const string Root = "api";
    private const string Version = "v{version:apiVersion}";
    private const string Base = Root + "/" + Version + "/";

    public static class Sql
    {
        private const string Endpoint = Base + "sql";

        public const string Get = Endpoint + "/get";

        public const string Save = Endpoint + "/save";
    }

    public static class Weather
    {
        private const string Endpoint = Base + "weather";

        public const string Get = Endpoint + "/get";

        public const string Save = Endpoint + "/save";
    }
}
