﻿using FluentValidation;
using UrlShortener.Core.Models.Requests;

namespace UrlShortener.WebApi.Models.Validators;

public class CustomUrlDtoValidator : AbstractValidator<CustomUrlDto>
{
    public CustomUrlDtoValidator()
    {
        RuleFor(x => x.FullUrl)
            .NotEmpty();

        RuleFor(x => x.ShortUrl)
            .NotEmpty();
    }
}