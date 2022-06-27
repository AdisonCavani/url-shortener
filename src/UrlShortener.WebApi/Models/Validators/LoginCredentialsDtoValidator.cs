﻿using FluentValidation;
using UrlShortener.Core.Models.Requests;

namespace UrlShortener.WebApi.Models.Validators;

public class LoginCredentialsDtoValidator : AbstractValidator<LoginCredentialsDto>
{
    public LoginCredentialsDtoValidator()
    {
        RuleFor(x => x.Email)
            .EmailAddress();

        RuleFor(x => x.Password)
            .NotEmpty();
    }
}