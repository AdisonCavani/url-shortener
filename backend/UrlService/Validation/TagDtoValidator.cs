using FluentValidation;
using UrlShortener.Shared.Contracts.Dtos;

namespace UrlService.Validation;

public class TagDtoValidator : AbstractValidator<TagDto> 
{
    public TagDtoValidator()
    {
        RuleFor(x => x.Name).MaximumLength(255);
    }
}