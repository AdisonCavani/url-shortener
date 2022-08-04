using FluentValidation;
using UrlShortener.Shared.Contracts.Requests;

namespace UrlService.Validation;

public class DeleteUserUrlRequestValidator : AbstractValidator<DeleteUserUrlRequest>
{
    public DeleteUserUrlRequestValidator()
    {
        RuleFor(x => x.Id)
            .NotNull()
            .GreaterThanOrEqualTo(1);
    }
}