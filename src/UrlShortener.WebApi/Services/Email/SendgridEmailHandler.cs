using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using UrlShortener.WebApi.Models.EmailTemplates;
using UrlShortener.WebApi.Models.Entities;
using UrlShortener.WebApi.Models.Settings;
using UrlShortener.WebApi.Services.Interfaces;

namespace UrlShortener.WebApi.Services.Email;

public class SendgridEmailHandler : IEmailHandler
{
    private readonly IEmailService _emailService;
    private readonly UserManager<AppUser> _userManager;
    private readonly IOptions<SendGridSettings> _sendGridSettings;

    public SendgridEmailHandler(IEmailService emailService, UserManager<AppUser> userManager,
        IOptions<SendGridSettings> sendGridSettings)
    {
        _emailService = emailService;
        _userManager = userManager;
        _sendGridSettings = sendGridSettings;
    }

    public async Task<bool> SendVerificationEmailAsync(AppUser user, CancellationToken ct = default)
    {
        var confirmationToken = await _userManager.GenerateEmailConfirmationTokenAsync(user);

        if (string.IsNullOrEmpty(confirmationToken))
            return false;

        var name = $"{user.FirstName} {user.LastName}";

        var templateData = new VerificationEmailTemplateData
        {
            Subject = "Confirm your email",
            Preheader = "",

            Token = confirmationToken
        };

        var emailSend = await _emailService.SendTemplateEmailAsync(
            name,
            user.Email,
            _sendGridSettings.Value.VerificationEmailTemplateId,
            templateData,
            ct);

        return emailSend;
    }

    public async Task<bool> SendPasswordChangedAlertAsync(AppUser user, CancellationToken ct = default)
    {
        var name = $"{user.FirstName} {user.LastName}";

        var templateData = new PasswordChangedAlertTemplateData
        {
            Subject = "Password has been changed",
            Preheader = "",

            FirstName = user.FirstName,
            LastName = user.LastName
        };

        var emailSend = await _emailService.SendTemplateEmailAsync(
            name,
            user.Email,
            _sendGridSettings.Value.PasswordChangedAlertTemplateId,
            templateData,
            ct);

        return emailSend;
    }

    public async Task<bool> SendPasswordRecoveryEmailAsync(AppUser user, CancellationToken ct = default)
    {
        var passwordToken = await _userManager.GeneratePasswordResetTokenAsync(user);

        if (string.IsNullOrEmpty(passwordToken))
            return false;

        var name = $"{user.FirstName} {user.LastName}";

        var templateData = new PasswordRecoveryTemplateData
        {
            Subject = "Password recovery",
            Preheader = "",

            Token = passwordToken
        };

        var emailSend = await _emailService.SendTemplateEmailAsync(
            name,
            user.Email,
            _sendGridSettings.Value.PasswordRecoveryTemplateId,
            templateData,
            ct);

        return emailSend;
    }
}