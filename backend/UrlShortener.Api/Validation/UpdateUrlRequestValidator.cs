using FluentValidation;
using UrlShortener.Shared.Contracts.Requests;

namespace UrlShortener.Api.Validation;

public class UpdateUrlRequestValidator : AbstractValidator<UpdateUrlRequest>
{
	public UpdateUrlRequestValidator()
	{
		RuleFor(x => x.Id).NotEmpty();
		RuleFor(x => x.NewUrl).NotEmpty();
	}
}
