using FluentValidation;
using UrlShortener.Api.Models.Validators;
using UrlShortener.Core.Models.Requests;

namespace UrlShortener.Api.Extensions.Startup;

public static class Validators
{
    public static void AddValidators(this IServiceCollection services)
    {
        ValidatorOptions.Global.LanguageManager.Enabled = false;

        services.AddScoped<IValidator<CustomUrlDto>, CustomUrlDtoValidator>();
    }
}