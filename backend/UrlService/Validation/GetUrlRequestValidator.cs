using FluentValidation;
using UrlShortener.Shared.Contracts.Requests;

namespace UrlService.Validation;

public class GetUrlRequestValidator : AbstractValidator<GetUrlRequest>
{
	public GetUrlRequestValidator()
	{
		RuleFor(x => x.Id)
			.NotEmpty()
			.Length(7);
	}
}
