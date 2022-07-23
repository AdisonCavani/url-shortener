using FluentValidation;
using UrlShortener.Shared.Contracts.Requests;

namespace UrlShortener.Api.Validation;

public class DeleteUrlRequestValidator : AbstractValidator<DeleteUrlRequest>
{
	public DeleteUrlRequestValidator()
	{
		RuleFor(x => x.Id).NotEmpty();
	}
}
