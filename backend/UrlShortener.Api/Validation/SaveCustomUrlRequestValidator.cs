using FluentValidation;
using UrlShortener.Shared.Contracts.Requests;

namespace UrlShortener.Api.Validation;

public class SaveCustomUrlRequestValidator : AbstractValidator<SaveCustomUrlRequest>
{
	public SaveCustomUrlRequestValidator()
	{
		RuleFor(x => x.ShortUrl).NotEmpty();
		RuleFor(x => x.FullUrl).NotEmpty();
	}
}
