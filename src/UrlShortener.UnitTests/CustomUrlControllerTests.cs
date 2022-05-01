using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using MockQueryable.Moq;
using Moq;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading;
using UrlShortener.Controllers.V1;
using UrlShortener.Models.App;
using UrlShortener.Models.Entities;
using Xunit;

namespace UrlShortener.UnitTests;

public class CustomUrlControllerTests
{
    // Dependencies
    private readonly Mock<AppDbContext> _context = new(new DbContextOptionsBuilder<AppDbContext>().Options);
    private readonly Mock<IDistributedCache> _cache = new();

    // Controller
    private readonly CustomUrlController _controller;

    public CustomUrlControllerTests()
    {
        _controller = new CustomUrlController(_context.Object, _cache.Object);

        // Mock
        var mockSet = GetUrls().AsQueryable().BuildMockDbSet();
        _context.Setup(m => m.CustomUrls).Returns(mockSet.Object);
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData(null)]
    public async void Get_WhenUrlIsEmptyNullOrWhitespace_ReturnsNotFound(string shortUrl)
    {
        // Arrange

        // Act
        var result = await _controller.Get(shortUrl);

        // Assert
        Assert.IsType<BadRequestResult>(result);
    }

    [Theory]
    [InlineData("wrongUrl")]
    public async void Get_WhenWrongUrl_ReturnsNotFound(string shortUrl)
    {
        // Arrange

        // Act
        var result = await _controller.Get(shortUrl);

        // Assert
        Assert.IsType<NotFoundResult>(result);
    }

    [Theory]
    [InlineData("shortUrl1", "https://fullUrl1.com")]
    [InlineData("shortUrl2", "https://fullUrl2.com")]
    [InlineData("shortUrl3", "https://fullUrl3.com")]
    [InlineData("shortUrl4", "https://fullUrl4.com")]
    [InlineData("shortUrl5", "https://fullUrl5.com")]
    public async void Get_WhenCorrectUrl_ReturnsOkObjectResult(string shortUrl, string expectedUrl)
    {
        // Arrange

        // Act
        var result = await _controller.Get(shortUrl);

        // Assert
        Assert.IsType<OkObjectResult>(result);

        var resultObj = result as OkObjectResult;
        Assert.Equal(expectedUrl, resultObj?.Value);
    }

    [Theory]
    [InlineData("shortUrl6", "https://fullUrl6.com")]
    [InlineData("shortUrl7", "https://fullUrl7.com")]
    [InlineData("shortUrl8", "https://fullUrl8.com")]
    [InlineData("shortUrl9", "https://fullUrl9.com")]
    [InlineData("shortUrl10", "https://fullUrl10.com")]
    public async void Save_WhenUnauthorized_ReturnsUnauthorizedResult(string shortUrl, string fullUrl)
    {
        // Arrange
        _context.Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(1); // Object saved

        // Act
        var result = await _controller.Save(new()
        {
            FullUrl = fullUrl,
            ShortUrl = shortUrl,
        });

        // Assert
        Assert.IsType<UnauthorizedResult>(result);
    }

    [Theory]
    [InlineData("shortUrl6", "https://fullUrl6.com")]
    [InlineData("shortUrl7", "https://fullUrl7.com")]
    [InlineData("shortUrl8", "https://fullUrl8.com")]
    [InlineData("shortUrl9", "https://fullUrl9.com")]
    [InlineData("shortUrl10", "https://fullUrl10.com")]
    public async void Save_WhenNotSaved_ReturnsInternalServerErrorResult(string shortUrl, string fullUrl)
    {
        // Arrange
        _context.Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(0); // Object not saved

        Mock<HttpContext> fakeHttpContext = new();

        GenericIdentity fakeIdentity = new("User");
        fakeIdentity.AddClaims(GetClaims());

        GenericPrincipal principal = new(fakeIdentity, null);

        fakeHttpContext.Setup(t => t.User).Returns(principal);
        ControllerContext controllerContext = new()
        {
            HttpContext = fakeHttpContext.Object,
        };

        _controller.ControllerContext = controllerContext;

        // Act
        var result = await _controller.Save(new()
        {
            FullUrl = fullUrl,
            ShortUrl = shortUrl,
        });

        // Assert
        var resultObj = result as StatusCodeResult;
        Assert.Equal(500, resultObj?.StatusCode);
    }

    [Theory]
    [InlineData("shortUrl1", "https://fullUrl1.com")]
    [InlineData("shortUrl1", "https://fullUrl2.com")]
    [InlineData("shortUrl3", "https://fullUrl3.com")]
    [InlineData("shortUrl4", "https://fullUrl4.com")]
    [InlineData("shortUrl5", "https://fullUrl5.com")]
    public async void Save_WhenTakenUrl_ReturnsConflictResult(string shortUrl, string fullUrl)
    {
        // Arrange
        _context.Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(1); // Object saved

        Mock<HttpContext> fakeHttpContext = new();

        GenericIdentity fakeIdentity = new("User");
        fakeIdentity.AddClaims(GetClaims());

        GenericPrincipal principal = new(fakeIdentity, null);

        fakeHttpContext.Setup(t => t.User).Returns(principal);
        ControllerContext controllerContext = new()
        {
            HttpContext = fakeHttpContext.Object,
        };

        _controller.ControllerContext = controllerContext;

        // Act
        var result = await _controller.Save(new()
        {
            FullUrl = fullUrl,
            ShortUrl = shortUrl,
        });

        // Assert
        Assert.IsType<ConflictResult>(result);
    }

    [Theory]
    [InlineData("shortUrl6", "https://fullUrl6.com")]
    [InlineData("shortUrl7", "https://fullUrl7.com")]
    [InlineData("shortUrl8", "https://fullUrl8.com")]
    [InlineData("shortUrl9", "https://fullUrl9.com")]
    [InlineData("shortUrl10", "https://fullUrl10.com")]
    public async void Save_WhenCorrectUrl_ReturnsCreatedResult(string shortUrl, string fullUrl)
    {
        // Arrange
        _context.Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(1); // Object saved

        Mock<HttpContext> fakeHttpContext = new();

        GenericIdentity fakeIdentity = new("User");
        fakeIdentity.AddClaims(GetClaims());

        GenericPrincipal principal = new(fakeIdentity, null);

        fakeHttpContext.Setup(t => t.User).Returns(principal);
        ControllerContext controllerContext = new()
        {
            HttpContext = fakeHttpContext.Object,
        };

        _controller.ControllerContext = controllerContext;

        // Act
        var result = await _controller.Save(new()
        {
            FullUrl = fullUrl,
            ShortUrl = shortUrl,
        });

        // Assert
        var resultObj = result as StatusCodeResult;
        Assert.Equal(201, resultObj?.StatusCode);
    }

    private static List<CustomUrl> GetUrls()
    {
        List<CustomUrl> urls = new()
        {
            new()
            {
                ShortUrl = "shortUrl1",
                FullUrl = "https://fullUrl1.com"
            },
            new()
            {
                ShortUrl = "shortUrl2",
                FullUrl = "https://fullUrl2.com"
            },
            new()
            {
                ShortUrl = "shortUrl3",
                FullUrl = "https://fullUrl3.com"
            },
            new()
            {
                ShortUrl = "shortUrl4",
                FullUrl = "https://fullUrl4.com"
            },
            new()
            {
                ShortUrl = "shortUrl5",
                FullUrl = "https://fullUrl5.com"
            }
        };

        return urls;
    }

    private static IEnumerable<Claim> GetClaims()
    {
        List<Claim> claims = new()
        {
            new Claim(ClaimTypes.NameIdentifier, "1"),
            new Claim(ClaimTypes.Role, "User")
        };

        return claims;
    }
}