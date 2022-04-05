namespace UrlShortener.Services;

public static class StartupServices
{
    public static IServiceCollection AddDependencyInjectionServices(this IServiceCollection services)
    {
        services.AddSingleton<WeatherService>();

        return services;
    }
}
