namespace UrlShortener.Contracts.V2;

public static class ApiRoutes
{
    private const string Root = "api";
    private const string Version = "v{version:apiVersion}";
    private const string Base = Root + "/" + Version + "/";

    public static class Account
    {
        private const string Endpoint = Base + "account";

        public const string Register = Endpoint + "/register";
    }

    public static class Config
    {
        private const string Endpoint = Base + "config";

        public const string Get = Endpoint + "/get";
    }

    public static class Url
    {
        private const string Endpoint = Base + "url";

        public const string Get = Endpoint + "/get/{shortUrl}";

        public const string Save = Endpoint + "/save";
    }
}
