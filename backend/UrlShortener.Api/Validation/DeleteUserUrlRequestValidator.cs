using FluentValidation;
using UrlShortener.Shared.Contracts.Requests;

namespace UrlShortener.Api.Validation;

public class DeleteUserUrlRequestValidator : AbstractValidator<DeleteUserUrlRequest>
{
    public DeleteUserUrlRequestValidator()
    {
        RuleFor(x => x.Id)
            .NotNull()
            .GreaterThanOrEqualTo(1);
    }
}