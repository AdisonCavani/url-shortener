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
}