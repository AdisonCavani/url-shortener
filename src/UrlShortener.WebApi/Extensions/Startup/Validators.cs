using FluentValidation;
using UrlShortener.Core.Models.Requests;
using UrlShortener.WebApi.Models.Validators;

namespace UrlShortener.WebApi.Extensions.Startup;

public static class Validators
{
    public static void AddValidators(this IServiceCollection services)
    {
        ValidatorOptions.Global.LanguageManager.Enabled = false;

        services.AddScoped<IValidator<CustomUrlDto>, CustomUrlDtoValidator>();
    }
}