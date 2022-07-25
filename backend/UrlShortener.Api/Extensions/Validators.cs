using FluentValidation;
using UrlShortener.Api.Validation;
using UrlShortener.Shared.Contracts.Requests;

namespace UrlShortener.Api.Extensions;

public static class Validators
{
    public static void AddValidators(this IServiceCollection services)
    {
        ValidatorOptions.Global.LanguageManager.Enabled = false;

        services.AddScoped<IValidator<GetUrlRequest>, GetUrlRequestValidator>();
        services.AddScoped<IValidator<SaveUrlRequest>, SaveUrlRequestValidator>();
        
        services.AddScoped<IValidator<GetAllUserUrlsRequest>, GetAllUserUrlsRequestValidator>();
    }
}