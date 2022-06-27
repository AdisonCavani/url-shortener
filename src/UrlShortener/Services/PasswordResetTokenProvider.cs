using Microsoft.AspNetCore.Identity;
using UrlShortener.Models.Entities;

namespace UrlShortener.Services;

public class PasswordResetTokenProvider : TotpSecurityStampBasedTokenProvider<AppUser>
{
    public const string ProviderKey = "ResetPassword";

    public override Task<bool> CanGenerateTwoFactorTokenAsync(UserManager<AppUser> manager, AppUser user)
    {
        return Task.FromResult(false);
    }
}