using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using UrlShortener.Data;
using UrlShortener.Entities;
using UrlShortener.Models;

namespace UrlShortener.Services;

public interface IAccountService
{
    Task<string?> GenerateJwt(LoginDto dto);
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

    public async Task<string?> GenerateJwt(LoginDto dto)
    {
        // Try to find user
        var user = await _context.Users
            .Include(u => u.Role)
            .FirstOrDefaultAsync(u => u.Email == dto.Email);

        if (user is null)
            return null;

        // Verify password
        var result = _passwordHasher.VerifyHashedPassword(user, user.PasswordHash, dto.Password);

        if (result == PasswordVerificationResult.Failed)
            return null;

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