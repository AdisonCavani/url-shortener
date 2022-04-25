using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using MockQueryable.Moq;
using Moq;
using System.Collections.Generic;
using System.Linq;
using UrlShortener.Controllers.V1;
using UrlShortener.Data;
using UrlShortener.Entities;
using UrlShortener.Models.Requests;
using UrlShortener.Services;
using Xunit;

namespace UrlShortener.Moq;

public class AccountControllerTests
{
    // Dependencies
    private readonly Mock<AppDbContext> _context = new(new DbContextOptionsBuilder<AppDbContext>().Options);
    private readonly Mock<AccountService> _accountService;
    private readonly Mock<IOptionsSnapshot<AuthSettings>> _authSettings = new();
    private readonly PasswordHasher<User> _passwordHasher = new();

    // Controller
    private readonly AccountController _accountController;

    public AccountControllerTests()
    {
        _accountService = new(_authSettings.Object);
        _accountController = new AccountController(_context.Object, _accountService.Object, _passwordHasher);

        // Mock
        var mockSet = GetUsers().AsQueryable().BuildMockDbSet();
        _context.Setup(m => m.Users).Returns(mockSet.Object);
        _authSettings.Setup(m => m.Value).Returns(new AuthSettings()
        {
            JwtExpireMinutes = 15,
            JwtIssuer = "localhost",
            JwtKey = "PRIVATE_KEY_DONT_SHARE"
        });
    }

    [Theory]
    [InlineData("test@email.com", "password")]
    public async void Login_WhenCorrectCredentials_ReturnsOk(string email, string password)
    {
        // Arrange
        LoginDto dto = new()
        {
            Email = email,
            Password = password
        };

        // Act
        var result = await _accountController.Login(dto);

        // Assert
        Assert.IsType<OkObjectResult>(result);
    }

    [Theory]
    [InlineData("wrong@email.com", "password")]
    public async void Login_WhenWrongEmail_ReturnsBadRequest(string email, string password)
    {
        // Arrange
        LoginDto dto = new()
        {
            Email = email,
            Password = password
        };

        // Act
        var result = await _accountController.Login(dto);

        // Assert
        Assert.IsType<BadRequestObjectResult>(result);
    }

    [Theory]
    [InlineData("test@email.com", "WrongPassword")]
    public async void Login_WhenWrongPassword_ReturnsBadRequest(string email, string password)
    {
        // Arrange
        LoginDto dto = new()
        {
            Email = email,
            Password = password
        };

        // Act
        var result = await _accountController.Login(dto);

        // Assert
        Assert.IsType<BadRequestObjectResult>(result);
    }

    [Theory]
    [InlineData("", "")]
    public async void Login_WhenEmptyCredentials_ReturnsBadRequest(string email, string password)
    {
        // Arrange
        LoginDto dto = new()
        {
            Email = email,
            Password = password
        };

        // Act
        var result = await _accountController.Login(dto);

        // Assert
        Assert.IsType<BadRequestObjectResult>(result);
    }

    [Theory]
    [InlineData(null, null)]
    public async void Login_WhenNullCredentials_ReturnsBadRequest(string email, string password)
    {
        // Arrange
        LoginDto dto = new()
        {
            Email = email,
            Password = password
        };

        // Act
        var result = await _accountController.Login(dto);

        // Assert
        Assert.IsType<BadRequestObjectResult>(result);
    }

    // Fake SQL data for mocking
    private static List<User> GetUsers()
    {
        List<User> users = new()
        {
            new()
            {
                Id = 1,
                Email = "test@email.com",
                PasswordHash = "AQAAAAEAACcQAAAAEK4Kw7rEOcgKMkf75W5rREz9DeMCrZ21Paxw7P3+pnEm/1oW45NuqH+pGtPT7lP3Ig==",
                RoleId = 1,
                Role = new()
                {
                    Id = 1,
                    Name = "User"
                }
            }
        };

        return users;
    }
}