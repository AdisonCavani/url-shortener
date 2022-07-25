using FluentValidation;
using UrlShortener.Shared.Contracts.Requests;

namespace UrlShortener.Api.Validation;

public class GetAllUserUrlsRequestValidator: AbstractValidator<GetAllUserUrlsRequest>
{
    public GetAllUserUrlsRequestValidator()
    {
        RuleFor(x => x.Page)
            .NotNull()
            .GreaterThanOrEqualTo(1);
    }
}