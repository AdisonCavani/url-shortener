using FluentValidation;
using UrlShortener.Shared.Contracts.Requests;

namespace UrlShortener.Api.Validation;

public class GetUserUrlRequestValidator : AbstractValidator<GetUserUrlRequest>
{
	public GetUserUrlRequestValidator()
	{
		RuleFor(x => x.Id)
			.NotNull()
			.GreaterThanOrEqualTo(1);
	}
}
