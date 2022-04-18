using Microsoft.AspNetCore.Identity;
using UrlShortener.Data;
using UrlShortener.Entities;
using UrlShortener.Models;

namespace UrlShortener.Services;

public interface IAccountService
{
    Task RegisterUser(RegisterUserDto dto);
}

public class AccountService : IAccountService
{
    private readonly AppDbContext _context;
    private readonly IPasswordHasher<User> _passwordHasher;

    public AccountService(AppDbContext context, IPasswordHasher<User> passwordHasher)
    {
        _context = context;
        _passwordHasher = passwordHasher;
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