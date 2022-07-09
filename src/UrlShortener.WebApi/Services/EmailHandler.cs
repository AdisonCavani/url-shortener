using Microsoft.AspNetCore.Identity;
using System.Web;
using UrlShortener.WebApi.Models.Entities;
using UrlShortener.WebApi.Services.Interfaces;

namespace UrlShortener.WebApi.Services;

// TODO: create HTML template handler
public class EmailHandler
{
    private readonly IEmailService _emailService;
    private readonly UserManager<AppUser> _userManager;

    public EmailHandler(IEmailService emailService, UserManager<AppUser> userManager)
    {
        _emailService = emailService;
        _userManager = userManager;
    }

    public async Task<bool> SendVerificationEmailAsync(AppUser user, CancellationToken token = default)
    {
        var confirmationToken = await _userManager.GenerateEmailConfirmationTokenAsync(user);

        if (string.IsNullOrEmpty(confirmationToken))
            return false;

        var name = $"{user.FirstName} {user.LastName}";
        var topic = "Confirm your email";

        var body = $"Token: {confirmationToken}";

        var emailSend = await _emailService.SendEmailAsync(name, user.Email, topic, body, token: token);

        return emailSend;
    }

    public async Task<bool> SendPasswordChangedAlertAsync(AppUser user, CancellationToken token = default)
    {
        var name = $"{user.FirstName} {user.LastName}";
        var topic = "Password has been changed";

        var body = $"<p>Your password has been changed</p>";

        var emailSend = await _emailService.SendEmailAsync(name, user.Email, topic, body, token: token);

        return emailSend;
    }

    public async Task<bool> SendPasswordRecoveryEmailAsync(AppUser user, CancellationToken token = default)
    {
        var passwordToken = await _userManager.GeneratePasswordResetTokenAsync(user);

        if (string.IsNullOrEmpty(passwordToken))
            return false;

        var name = $"{user.FirstName} {user.LastName}";
        var topic = "Password recovery";

        var body = $"<p>Token: {HttpUtility.UrlEncode(passwordToken)}</p>";

        var emailSend = await _emailService.SendEmailAsync(name, user.Email, topic, body, token: token);

        return emailSend;
    }
}