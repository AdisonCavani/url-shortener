﻿using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using UrlShortener.Core.Models.Responses;
using UrlShortener.WebApi.Models.Entities;
using UrlShortener.WebApi.Models.Settings;

namespace UrlShortener.WebApi.Services;

public class JwtService
{
    private readonly IOptionsSnapshot<AuthSettings> _authSettings;

    public JwtService(IOptionsSnapshot<AuthSettings> authSettings)
    {
        _authSettings = authSettings;
    }

    public JwtTokenDto GenerateToken(AppUser user)
    {
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_authSettings.Value.SecretKey));

        SecurityTokenDescriptor tokenDescriptor = new()
        {
            Subject = new(new Claim[]
            {
                new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                new(JwtRegisteredClaimNames.Email, user.Email),
                new(JwtRegisteredClaimNames.GivenName, user.FirstName),
                new(JwtRegisteredClaimNames.FamilyName, user.LastName)
            }),
            Expires = DateTime.UtcNow.AddMinutes(_authSettings.Value.ExpireMinutes),
            Audience = _authSettings.Value.Audience,
            Issuer = _authSettings.Value.Issuer,
            SigningCredentials = new(key, SecurityAlgorithms.HmacSha256Signature)
        };

        var tokenHandler = new JwtSecurityTokenHandler();
        var token = tokenHandler.CreateToken(tokenDescriptor);

        return new()
        {
            Token = tokenHandler.WriteToken(token),
        };
    }
}