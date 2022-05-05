using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using UrlShortener.Models.App;
using UrlShortener.Models.Entities;

namespace UrlShortener.Services;

public interface IAccountService
{
    string GenerateJwt(User user);
}

public class AccountService : IAccountService
{
    private readonly IOptionsSnapshot<AuthSettings> _authSettings;

    public AccountService(IOptionsSnapshot<AuthSettings> authSettings)
    {
        _authSettings = authSettings;
    }

    public string GenerateJwt(User user)
    {
        // Add JWT claims
        var claims = new List<Claim>()
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Role, user.RoleId.ToString())
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_authSettings.Value.JwtKey));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            _authSettings.Value.JwtIssuer,
            _authSettings.Value.JwtIssuer,
            claims,
            DateTime.UtcNow,
            DateTime.Now.AddMinutes(_authSettings.Value.JwtExpireMinutes),
            credentials);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}