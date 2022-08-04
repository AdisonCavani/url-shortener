using FluentValidation;
using UrlShortener.Shared.Contracts.Requests;

namespace UrlService.Validation;

public class GetAllUserUrlsRequestValidator: AbstractValidator<GetAllUserUrlsRequest>
{
    public GetAllUserUrlsRequestValidator()
    {
        RuleFor(x => x.Page)
            .NotNull()
            .GreaterThanOrEqualTo(1);
    }
}