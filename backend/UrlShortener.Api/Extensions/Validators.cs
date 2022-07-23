using FluentValidation;
using UrlShortener.Api.Validation;
using UrlShortener.Shared.Contracts.Requests;

namespace UrlShortener.Api.Extensions;

public static class Validators
{
    public static void AddValidators(this IServiceCollection services)
    {
        ValidatorOptions.Global.LanguageManager.Enabled = false;
        
        services.AddScoped<IValidator<DeleteUrlRequest>, DeleteUrlRequestValidator>();
        services.AddScoped<IValidator<GetAllUrlsRequest>, GetAllUrlsRequestValidator>();
        services.AddScoped<IValidator<GetUrlRequest>, GetUrlRequestValidator>();
        services.AddScoped<IValidator<SaveUrlRequest>, SaveUrlRequestValidator>();
        services.AddScoped<IValidator<UpdateUrlRequest>, UpdateUrlRequestValidator>();
    }
}