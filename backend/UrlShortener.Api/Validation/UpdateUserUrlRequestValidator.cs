using FluentValidation;
using UrlShortener.Shared.Contracts.Requests;

namespace UrlShortener.Api.Validation;

public class UpdateUserUrlRequestValidator : AbstractValidator<UpdateUserUrlRequest>
{
	public UpdateUserUrlRequestValidator()
	{
		RuleFor(x => x.Id).NotEmpty();
		RuleFor(x => x.NewUrl).NotEmpty();
	}
}
