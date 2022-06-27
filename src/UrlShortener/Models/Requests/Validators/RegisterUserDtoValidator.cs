using FluentValidation;
using UrlShortener.Models.App;

namespace UrlShortener.Models.Requests.Validators;

public class RegisterUserDtoValidator : AbstractValidator<RegisterUserDto>
{
    public RegisterUserDtoValidator(AppDbContext context)
    {
        RuleFor(x => x.Email)
            .NotEmpty()
            .EmailAddress();

        RuleFor(x => x.Password)
            .NotEmpty()
            .MinimumLength(8)
            .Matches(@"[A-Z]+").WithMessage("Password must contain at least one uppercase letter.")
            .Matches(@"[a-z]+").WithMessage("Password must contain at least one lowercase letter.")
            .Matches(@"[0-9]+").WithMessage("Password must contain at least one number.");

        RuleFor(x => x.Email)
            .Custom((value, validation) =>
            {
                var emailUsed = context.Users.Any(u => u.Email == value);

                if (emailUsed)
                    validation.AddFailure("Email", "Email is taken");
            });
    }
}
