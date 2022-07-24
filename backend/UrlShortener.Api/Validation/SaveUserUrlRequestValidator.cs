using FluentValidation;
using UrlShortener.Shared.Contracts.Requests;

namespace UrlShortener.Api.Validation;

public class SaveUserUrlRequestValidator : AbstractValidator<SaveUserUrlRequest>
{
	public SaveUserUrlRequestValidator()
	{
		RuleFor(x => x.Url).NotEmpty();
	}
}
