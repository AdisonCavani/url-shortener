using FluentValidation;
using UrlShortener.Core.Models.Requests;

namespace UrlShortener.Models.Validators;

public class ResendVerificationEmailDtoValidator : AbstractValidator<ResendVerificationEmailDto>
{
    public ResendVerificationEmailDtoValidator()
    {
        RuleFor(x => x.Email)
            .EmailAddress();
    }
}
