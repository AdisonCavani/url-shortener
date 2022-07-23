using FluentValidation;
using UrlShortener.Shared.Contracts.Requests;

namespace UrlShortener.Api.Validation;

public class GetAllUrlsRequestValidator : AbstractValidator<GetAllUrlsRequest>
{
	public GetAllUrlsRequestValidator()
	{
		RuleFor(x => x.Page)
			.NotNull()
			.GreaterThan(0);
	}
}
