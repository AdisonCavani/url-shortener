using FluentValidation;
using UrlShortener.Core.Models.Requests;

namespace UrlShortener.WebApi.Models.Validators;

public class IsEmailConfirmedDtoValidator : AbstractValidator<IsEmailConfirmedDto>
{
    public IsEmailConfirmedDtoValidator()
    {
        RuleFor(x => x.Email)
            .EmailAddress();
    }
}