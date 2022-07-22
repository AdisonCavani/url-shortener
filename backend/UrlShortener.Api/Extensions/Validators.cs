using FluentValidation;
using UrlShortener.Api.Validation;
using UrlShortener.Shared.Contracts.Requests;

namespace UrlShortener.Api.Extensions;

public static class Validators
{
    public static void AddValidators(this IServiceCollection services)
    {
        ValidatorOptions.Global.LanguageManager.Enabled = false;

        services.AddScoped<IValidator<GetCustomUrlRequest>, GetCustomUrlRequestValidator>();
        services.AddScoped<IValidator<GetUrlRequest>, GetUrlRequestValidator>();
        services.AddScoped<IValidator<SaveCustomUrlRequest>, SaveCustomUrlRequestValidator>();
        services.AddScoped<IValidator<SaveUrlRequest>, SaveUrlRequestValidator>();
    }
}