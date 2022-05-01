using FluentValidation;
using HashidsNet;
using Microsoft.AspNetCore.Identity;
using UrlShortener.Models.Entities;
using UrlShortener.Models.Requests;
using UrlShortener.Models.Requests.Validators;
using UrlShortener.Services;

namespace UrlShortener.Extensions;

public static class StartupServices
{
    public static IServiceCollection AddDependencyInjectionServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<AccountSeeder>();
        services.AddScoped<IAccountService, AccountService>();
        services.AddScoped<IPasswordHasher<User>, PasswordHasher<User>>();
        services.AddScoped<IValidator<CustomUrlDto>, CustomUrlDtoValidator>();
        services.AddScoped<IValidator<RegisterUserDto>, RegisterUserDtoValidator>();

        services.AddSingleton<IHashids>(_ => new Hashids(configuration["AppSettings:HashidsSalt"], 7));

        // Redis cache
        services.AddStackExchangeRedisCache(options =>
        {
            options.Configuration = configuration["AppSettings:RedisConnection"];
            options.ConfigurationOptions = new()
            {
                // TODO: configure
            };
        });
        services.AddDistributedMemoryCache();

        return services;
    }
}
