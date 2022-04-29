using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using MockQueryable.Moq;
using Moq;
using System.Collections.Generic;
using System.Linq;
using UrlShortener.Controllers.V1;
using UrlShortener.Data;
using UrlShortener.Entities;
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
    [InlineData(null)]
    public async void Get_WhenUrlIsNull_ReturnsNotFound(string shortUrl)
    {
        // Arrange

        // Act
        var result = await _controller.Get(shortUrl);

        // Assert
        Assert.IsType<BadRequestResult>(result);
    }

    [Theory]
    [InlineData("")]
    public async void Get_WhenUrlIsEmpty_ReturnsNotFound(string shortUrl)
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
    [InlineData("shortUrl")]
    public async void Get_WhenCorrectUrl_ReturnsOkObjectResult(string shortUrl)
    {
        // Arrange

        // Act
        var result = await _controller.Get(shortUrl);

        // Assert
        Assert.IsType<OkObjectResult>(result);

        var resultObj = result as OkObjectResult;
        Assert.Equal("https://fullUrl.com", resultObj?.Value);
    }

    [Theory]
    [InlineData(null, "shortUrl")]
    public async void Save_WhenFullUrlIsNull_ReturnsBadRequest(string fullUrl, string shortUrl)
    {
        // Arrange

        // Act
        var result = await _controller.Save(fullUrl, shortUrl);

        // Assert
        Assert.IsType<BadRequestResult>(result);
    }

    [Theory]
    [InlineData("fullUrl", null)]
    public async void Save_WhenShortUrlIsNull_ReturnsBadRequest(string fullUrl, string shortUrl)
    {
        // Arrange

        // Act
        var result = await _controller.Save(fullUrl, shortUrl);

        // Assert
        Assert.IsType<BadRequestResult>(result);
    }

    [Theory]
    [InlineData(null, null)]
    public async void Save_WhenBothUrlAreNull_ReturnsBadRequest(string fullUrl, string shortUrl)
    {
        // Arrange

        // Act
        var result = await _controller.Save(fullUrl, shortUrl);

        // Assert
        Assert.IsType<BadRequestResult>(result);
    }

    [Theory]
    [InlineData("", "shortUrl")]
    public async void Save_WhenFullUrlIsEmpty_ReturnsBadRequest(string fullUrl, string shortUrl)
    {
        // Arrange

        // Act
        var result = await _controller.Save(fullUrl, shortUrl);

        // Assert
        Assert.IsType<BadRequestResult>(result);
    }

    [Theory]
    [InlineData("fullUrl", "")]
    public async void Save_WhenShortUrlIsEmpty_ReturnsBadRequest(string fullUrl, string shortUrl)
    {
        // Arrange

        // Act
        var result = await _controller.Save(fullUrl, shortUrl);

        // Assert
        Assert.IsType<BadRequestResult>(result);
    }

    [Theory]
    [InlineData("", "")]
    public async void Save_WhenBothUrlAreEmpty_ReturnsBadRequest(string fullUrl, string shortUrl)
    {
        // Arrange

        // Act
        var result = await _controller.Save(fullUrl, shortUrl);

        // Assert
        Assert.IsType<BadRequestResult>(result);
    }

    [Theory]
    [InlineData("   ", "shortUrl")]
    public async void Save_WhenFullUrlIsWhitespace_ReturnsBadRequest(string fullUrl, string shortUrl)
    {
        // Arrange

        // Act
        var result = await _controller.Save(fullUrl, shortUrl);

        // Assert
        Assert.IsType<BadRequestResult>(result);
    }

    [Theory]
    [InlineData("fullUrl", "   ")]
    public async void Save_WhenShortUrlIsWhitespace_ReturnsBadRequest(string fullUrl, string shortUrl)
    {
        // Arrange

        // Act
        var result = await _controller.Save(fullUrl, shortUrl);

        // Assert
        Assert.IsType<BadRequestResult>(result);
    }

    [Theory]
    [InlineData("   ", "   ")]
    public async void Save_WhenBothUrlsAreWhitespace_ReturnsBadRequest(string fullUrl, string shortUrl)
    {
        // Arrange

        // Act
        var result = await _controller.Save(fullUrl, shortUrl);

        // Assert
        Assert.IsType<BadRequestResult>(result);
    }

    private static List<CustomUrl> GetUrls()
    {
        List<CustomUrl> urls = new()
        {
            new()
            {
                ShortUrl = "shortUrl",
                FullUrl = "https://fullUrl.com"
            }
        };

        return urls;
    }
}