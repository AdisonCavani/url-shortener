using FluentValidation;
using UrlShortener.Core.Models.Requests;

namespace UrlShortener.WebApi.Models.Validators;

public class PasswordRecoveryTokenDtoValidator : AbstractValidator<PasswordRecoveryTokenDto>
{
    public PasswordRecoveryTokenDtoValidator()
    {
        Include(new PasswordRecoveryDtoValidator());

        RuleFor(x => x.Token)
            .NotEmpty();
    }
}
