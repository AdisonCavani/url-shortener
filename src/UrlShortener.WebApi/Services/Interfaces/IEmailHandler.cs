using UrlShortener.WebApi.Models.Entities;

namespace UrlShortener.WebApi.Services.Interfaces;

public interface IEmailHandler
{
    Task<bool> SendVerificationEmailAsync(AppUser user, CancellationToken ct = default);
    Task<bool> SendPasswordChangedAlertAsync(AppUser user, CancellationToken ct = default);
    Task<bool> SendPasswordRecoveryEmailAsync(AppUser user, CancellationToken ct = default);
}