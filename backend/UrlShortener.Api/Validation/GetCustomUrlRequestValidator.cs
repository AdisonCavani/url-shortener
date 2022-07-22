using FluentValidation;
using UrlShortener.Shared.Contracts.Requests;

namespace UrlShortener.Api.Validation;

public class GetCustomUrlRequestValidator : AbstractValidator<GetCustomUrlRequest>
{
	public GetCustomUrlRequestValidator()
	{
		RuleFor(x => x.Url).NotEmpty();
	}
}
