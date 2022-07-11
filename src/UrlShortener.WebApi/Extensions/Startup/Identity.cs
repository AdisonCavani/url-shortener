using Microsoft.AspNetCore.Identity;
using UrlShortener.WebApi.Models.App;
using UrlShortener.WebApi.Models.Entities;
using UrlShortener.WebApi.Services;
using UrlShortener.WebApi.Services.Auth;

namespace UrlShortener.WebApi.Extensions.Startup;

public static class Identity
{
    public static void ConfigureIdentity(this IServiceCollection services)
    {
        services.AddIdentity<AppUser, AppRole>(options =>
            {
                options.Tokens.PasswordResetTokenProvider = PasswordResetTokenProvider.ProviderKey;
                options.Tokens.EmailConfirmationTokenProvider = EmailConfirmationTokenProvider.ProviderKey;
            })
            .AddEntityFrameworkStores<AppDbContext>()
            .AddTokenProvider<DataProtectorTokenProvider<AppUser>>(TokenOptions.DefaultProvider)
            .AddTokenProvider<PasswordResetTokenProvider>(PasswordResetTokenProvider.ProviderKey)
            .AddTokenProvider<EmailConfirmationTokenProvider>(EmailConfirmationTokenProvider.ProviderKey);

        services.Configure<IdentityOptions>(options =>
        {
            options.User.RequireUniqueEmail = true;

            options.SignIn.RequireConfirmedEmail = true;

            options.Lockout.AllowedForNewUsers = true;
            options.Lockout.MaxFailedAccessAttempts = 5;
            options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromSeconds(15);

            options.Password.RequireDigit = true;
            options.Password.RequiredLength = 8;
            options.Password.RequireLowercase = true;
            options.Password.RequireUppercase = true;
            options.Password.RequireNonAlphanumeric = true;
        });
    }
}