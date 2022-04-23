using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using UrlShortener.Data;
using UrlShortener.Entities;
using UrlShortener.Models.Requests;

namespace UrlShortener.Services;

public interface IAccountService
{
    string GenerateJwt(User user);
    Task RegisterUser(RegisterUserDto dto);
}

public class AccountService : IAccountService
{
    private readonly AppDbContext _context;
    private readonly IPasswordHasher<User> _passwordHasher;
    private readonly IOptionsSnapshot<AuthSettings> _authSettings;

    public AccountService(AppDbContext context, IPasswordHasher<User> passwordHasher, IOptionsSnapshot<AuthSettings> authSettings)
    {
        _context = context;
        _authSettings = authSettings;
        _passwordHasher = passwordHasher;
    }

    public string GenerateJwt(User user)
    {
        // Add JWT claims
        var claims = new List<Claim>()
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Role, user.Role.Name)
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

    public async Task RegisterUser(RegisterUserDto dto)
    {
        var newUser = new User()
        {
            Email = dto.Email,
            RoleId = 1
        };

        var hashedPassword = _passwordHasher.HashPassword(newUser, dto.Password);
        newUser.PasswordHash = hashedPassword;

        await _context.Users.AddAsync(newUser);
        await _context.SaveChangesAsync();
    }
}