namespace ManagementService.Contracts;

public static class ApiRoutes
{
    private const string Base = "/api";

    public const string Health = $"{Base}/health";

    public static class Basic
    {
        public const string Create = $"{Base}/create";
        public const string Get = $"{Base}/get";
    }
}