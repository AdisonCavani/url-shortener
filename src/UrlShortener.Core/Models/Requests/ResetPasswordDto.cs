﻿namespace UrlShortener.Core.Models.Requests;

public class ResetPasswordDto : PasswordRecoveryTokenDto
{
    public string NewPassword { get; init; }
}
