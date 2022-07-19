using System.Net;
using System.Text.Json;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

namespace UrlShortener.WebApi.Extensions.Startup;

public static class Authentication
{
    public static void AddAuthentication(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddAuthentication(options =>
        {
            options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;

        }).AddJwtBearer(options =>
        {
            options.SaveToken = true;
            options.RequireHttpsMetadata = true;
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateLifetime = true,
                ValidateAudience = true,
                ValidateIssuer = true,
                ValidateIssuerSigningKey = true,
                ClockSkew = TimeSpan.Zero, // FIX: might cause issues, if auth is out of sync
                ValidIssuer = configuration["AuthSettings:Issuer"],
                ValidAudience = configuration["AuthSettings:Audience"],
                IssuerSigningKeyResolver = (s, securityToken, identifier, parameters) =>
                {
                    var json = new WebClient().DownloadString($"{parameters.ValidIssuer}/.well-known/jwks.json");
                    var obj = JsonSerializer.Deserialize<JsonWebKeySet>(json);
                    return obj?.Keys;
                }
            };
        });
    }
}