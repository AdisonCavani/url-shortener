﻿using FluentValidation;
using UrlShortener.Core.Models.Requests;

namespace UrlShortener.WebApi.Models.Validators;

public class ConfirmEmailDtoValidator : AbstractValidator<ConfirmEmailDto>
{
    public ConfirmEmailDtoValidator()
    {
        RuleFor(x => x.UserId)
            .NotEmpty();

        RuleFor(x => x.Token)
            .NotEmpty();
    }
}