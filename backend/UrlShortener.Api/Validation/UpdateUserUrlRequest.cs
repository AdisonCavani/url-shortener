using FluentValidation;
using UrlShortener.Shared.Contracts.Requests;

namespace UrlShortener.Api.Validation;

public class UpdateUserUrlRequestValidator : AbstractValidator<UpdateUserUrlRequest>
{
    public UpdateUserUrlRequestValidator()
    {
        RuleFor(x => x.Id)
            .NotNull()
            .GreaterThanOrEqualTo(1);
        
        RuleFor(x => x.Url)
            .NotEmpty()
            .MaximumLength(255)
            .Custom((value, context) =>
            {
                if (!Uri.TryCreate(value, UriKind.Absolute, out var uriResult) 
                    || !(uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps))
                    context.AddFailure("Url", "Must be valid URL");
            });

        RuleFor(x => x.Title).MaximumLength(255);

        RuleForEach(x => x.Tags).SetValidator(new TagDtoValidator());
    }
}