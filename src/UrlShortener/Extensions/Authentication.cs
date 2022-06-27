using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace UrlShortener.Extensions;

public static class Authentication
{
    public static void AddAuthentication(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddAuthentication(options =>
        {
            options.DefaultScheme = "Bearer";
            options.DefaultChallengeScheme = "Bearer";
            options.DefaultAuthenticateScheme = "Bearer";

        }).AddJwtBearer(options =>
        {
            options.SaveToken = true;
            options.RequireHttpsMetadata = true;
            options.TokenValidationParameters = new TokenValidationParameters()
            {
                ValidateLifetime = true,
                ValidateAudience = true,
                ValidateIssuer = true,
                ValidateIssuerSigningKey = true,
                ClockSkew = TimeSpan.Zero, // FIX: might cause issues, if auth is out of sync
                ValidIssuer = configuration["AuthSettings:JwtIssuer"],
                ValidAudience = configuration["AuthSettings:JwtIssuer"],
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["AuthSettings:JwtKey"]))
            };
        });
    }
}