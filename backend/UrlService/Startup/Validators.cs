using FluentValidation;

namespace UrlService.Startup;

public static class Validators
{
    public static void AddValidators(this IServiceCollection services)
    {
        ValidatorOptions.Global.LanguageManager.Enabled = false;

        // services.AddScoped<IValidator<GetUrlRequest>, GetUrlRequestValidator>();
    }
}