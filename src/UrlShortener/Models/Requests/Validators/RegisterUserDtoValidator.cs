using FluentValidation;
using UrlShortener.Data;
using UrlShortener.Models.Requests;

namespace UrlShortener.Models.Validators;

public class RegisterUserDtoValidator : AbstractValidator<RegisterUserDto>
{
    public RegisterUserDtoValidator(AppDbContext context)
    {
        RuleFor(x => x.Email)
            .NotEmpty()
            .EmailAddress();

        RuleFor(x => x.Password)
            .MinimumLength(6);

        RuleFor(x => x.Email)
            .Custom((value, validation) =>
            {
                var emailUsed = context.Users.Any(u => u.Email == value);

                if (emailUsed)
                    validation.AddFailure("Email", "Email is taken");
            });
    }
}
