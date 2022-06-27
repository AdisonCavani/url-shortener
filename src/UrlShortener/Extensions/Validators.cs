using FluentValidation;
using UrlShortener.Models.Requests;
using UrlShortener.Models.Requests.Validators;

namespace UrlShortener.Extensions;

public static class Validators
{
    public static void AddValidators(this IServiceCollection services)
    {
        services.AddScoped<IValidator<CustomUrlDto>, CustomUrlDtoValidator>();
        services.AddScoped<IValidator<LoginDto>, LoginDtoValidator>();
        services.AddScoped<IValidator<RegisterUserDto>, RegisterUserDtoValidator>();
    }
}