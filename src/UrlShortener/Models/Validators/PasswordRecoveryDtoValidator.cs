using FluentValidation;
using UrlShortener.Core.Models.Requests;

namespace UrlShortener.Models.Validators;

public class PasswordRecoveryDtoValidator : AbstractValidator<PasswordRecoveryDto>
{
    public PasswordRecoveryDtoValidator()
    {
        RuleFor(x => x.Email)
            .EmailAddress();
    }
}
