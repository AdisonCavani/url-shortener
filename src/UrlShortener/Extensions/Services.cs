using HashidsNet;
using Microsoft.AspNetCore.Identity;
using UrlShortener.Models.Entities;
using UrlShortener.Services;

namespace UrlShortener.Extensions;

public static class Services
{
    public static void RegisterServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<IAccountService, AccountService>();
        services.AddScoped<IPasswordHasher<User>, PasswordHasher<User>>();

        services.AddSingleton<IHashids>(_ => new Hashids(configuration["AppSettings:HashidsSalt"], 7));
    }
}