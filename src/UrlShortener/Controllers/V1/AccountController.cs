using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UrlShortener.Contracts.V1;
using UrlShortener.Data;
using UrlShortener.Entities;
using UrlShortener.Models;
using UrlShortener.Services;

namespace UrlShortener.Controllers.V1;

[ApiController]
[ApiVersion("1")]
public class AccountController : ControllerBase
{
    private readonly AppDbContext _context;
    private readonly IAccountService _accountService;
    private readonly IPasswordHasher<User> _passwordHasher;

    public AccountController(AppDbContext context, IAccountService accountService, IPasswordHasher<User> passwordHasher)
    {
        _context = context;
        _accountService = accountService;
        _passwordHasher = passwordHasher;
    }

    [HttpPost(ApiRoutes.Account.Login)]
    public async Task<IActionResult> Login([FromBody] LoginDto dto)
    {
        // Try to find user
        var user = await _context.Users
            .Include(u => u.Role)
            .FirstOrDefaultAsync(u => u.Email == dto.Email);

        if (user is null)
            return BadRequest("Invalid email or password");

        // Verify password
        var result = _passwordHasher.VerifyHashedPassword(user, user.PasswordHash, dto.Password);

        if (result == PasswordVerificationResult.Failed)
            return BadRequest("Invalid email or password");

        var token = _accountService.GenerateJwt(user);

        return Ok(token);
    }

    [HttpPost(ApiRoutes.Account.Register)]
    public async Task<IActionResult> RegisterUser([FromBody] RegisterUserDto dto)
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

        return Ok();
    }
}