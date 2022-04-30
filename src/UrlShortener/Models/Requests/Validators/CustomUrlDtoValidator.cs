using FluentValidation;

namespace UrlShortener.Models.Requests.Validators;

public class CustomUrlDtoValidator : AbstractValidator<CustomUrlDto>
{
    public CustomUrlDtoValidator()
    {
        RuleFor(x => x.FullUrl)
            .NotEmpty();

        RuleFor(x => x.ShortUrl)
            .NotEmpty();
    }
}