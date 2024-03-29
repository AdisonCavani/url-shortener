﻿namespace UrlShortener.Shared.Contracts;

public static class ApiRoutes
{
    private const string Base = "/api";

    public const string Health = $"{Base}/health";
    
    public static class Config
    {
        private const string Endpoint = $"{Base}/config";

        public const string Get = $"{Endpoint}/get";
    }
    
    public static class Url
    {
        private const string Endpoint = $"{Base}/url";

        public const string Get = $"{Endpoint}/get";
        public const string Save = $"{Endpoint}/save";
    }

    public static class UserUrl
    {
        private const string Endpoint = $"{Base}/userUrl";

        public const string Get = $"{Endpoint}/get";
        public const string GetAll = $"{Endpoint}/getAll";
        public const string Save = $"{Endpoint}/save";
        public const string Update = $"{Endpoint}/update";
        public const string Delete = $"{Endpoint}/delete";
    }
}