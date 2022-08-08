using System.Net;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using UrlService.Options;

namespace UrlService.Startup;

public static class Authentication
{
    public static void AddAuthentication(this IServiceCollection services, AuthOptions authOptions)
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
                ValidIssuer = authOptions.Issuer,
                ValidAudience = authOptions.Audience,
                IssuerSigningKeyResolver = (_, _, _, parameters) =>
                {
                    var json = new WebClient().DownloadString($"{parameters.ValidIssuer}/.well-known/jwks.json");
                    var obj = JsonConvert.DeserializeObject<JsonWebKeySet>(json);
                    return obj?.Keys;
                }
            };
        });
    }
}