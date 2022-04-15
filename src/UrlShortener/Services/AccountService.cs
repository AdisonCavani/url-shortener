using UrlShortener.Data;
using UrlShortener.Entities;
using UrlShortener.Models;

namespace UrlShortener.Services;

public interface IAccountService
{
    void RegisterUser(RegisterUserDto dto);
}

public class AccountService : IAccountService
{
    private readonly AppDbContext _context;
    
    public AccountService(AppDbContext context)
    {
        _context = context;
    }
    
    public async void RegisterUser(RegisterUserDto dto)
    {
        var newUser = new User()
        {
            Email = dto.Email,
            DateOfBirth = dto.DateOfBirth,
            RoleId = dto.RoleId
        };

        await _context.Users.AddAsync(newUser);
        await _context.SaveChangesAsync();
    }
}