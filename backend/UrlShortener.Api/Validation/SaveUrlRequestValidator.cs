using FluentValidation;
using UrlShortener.Shared.Contracts.Requests;

namespace UrlShortener.Api.Validation;

public class SaveUrlRequestValidator : AbstractValidator<SaveUrlRequest>
{
	public SaveUrlRequestValidator()
	{
		RuleFor(x => x.Url).NotEmpty();
	}
}
