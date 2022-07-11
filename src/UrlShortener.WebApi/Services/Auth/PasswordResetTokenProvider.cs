using Microsoft.AspNetCore.Identity;
using UrlShortener.WebApi.Models.Entities;

namespace UrlShortener.WebApi.Services.Auth;

public class PasswordResetTokenProvider : TotpSecurityStampBasedTokenProvider<AppUser>
{
    public const string ProviderKey = "ResetPassword";

    public override Task<bool> CanGenerateTwoFactorTokenAsync(UserManager<AppUser> manager, AppUser user)
    {
        return Task.FromResult(false);
    }
}