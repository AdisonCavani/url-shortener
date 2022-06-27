using FluentValidation;
using UrlShortener.Core.Models.Requests;

namespace UrlShortener.WebApi.Models.Validators;

public class ResendVerificationEmailDtoValidator : AbstractValidator<ResendVerificationEmailDto>
{
    public ResendVerificationEmailDtoValidator()
    {
        RuleFor(x => x.Email)
            .EmailAddress();
    }
}
