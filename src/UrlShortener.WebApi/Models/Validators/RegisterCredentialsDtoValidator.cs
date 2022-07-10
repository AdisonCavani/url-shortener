using FluentValidation;
using UrlShortener.Core.Models.Requests;

namespace UrlShortener.WebApi.Models.Validators;

public class RegisterCredentialsDtoValidator : AbstractValidator<RegisterCredentialsDto>
{
    public RegisterCredentialsDtoValidator()
    {
        RuleLevelCascadeMode = CascadeMode.Stop;

        RuleFor(x => x.FirstName)
            .NotEmpty();

        RuleFor(x => x.LastName)
            .NotEmpty();

        RuleFor(x => x.Email)
            .NotEmpty()
            .EmailAddress();

        RuleFor(x => x.Password)
            .NotEmpty()
            .MinimumLength(8)
            .Matches(@"[A-Z]+").WithMessage("Password must contain at least one uppercase letter.")
            .Matches(@"[a-z]+").WithMessage("Password must contain at least one lowercase letter.")
            .Matches(@"[0-9]+").WithMessage("Password must contain at least one number.")
            .Custom((value, validation) =>
            {
                if (value.All(char.IsLetterOrDigit))
                    validation.AddFailure("Password must contain at least one non alphanumeric character.");
            });

        // TODO: Add better password validation
    }
}
