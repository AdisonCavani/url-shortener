using HashidsNet;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using MockQueryable.Moq;
using Moq;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using UrlShortener.Controllers.V1;
using UrlShortener.Data;
using UrlShortener.Entities;
using Xunit;

namespace UrlShortener.UnitTests;

public class UrlControllerTests
{
    // Dependencies
    private readonly Mock<AppDbContext> _context = new(new DbContextOptionsBuilder<AppDbContext>().Options);
    private readonly Mock<IDistributedCache> _cache = new();
    private readonly Hashids _hashids = new("salt", 7);

    // Controller
    private readonly UrlController _urlController;

    public UrlControllerTests()
    {
        _urlController = new UrlController(_context.Object, _cache.Object, _hashids);

        // Mock
        var mockSet = GetUrls().AsQueryable().BuildMockDbSet();
        _context.Setup(m => m.Urls).Returns(mockSet.Object);
    }

    [Theory]
    [InlineData("link")]
    public async void Get_WhenTooShortUrl_ReturnsBadRequest(string shortUrl)
    {
        // Arrange

        // Act
        var result = await _urlController.Get(shortUrl);

        // Assert
        Assert.IsType<BadRequestResult>(result);
    }

    [Theory]
    [InlineData("toolonglink")]
    public async void Get_WhenTooLongUrl_ReturnsBadRequest(string shortUrl)
    {
        // Arrange

        // Act
        var result = await _urlController.Get(shortUrl);

        // Assert
        Assert.IsType<BadRequestResult>(result);
    }

    [Theory]
    [InlineData("abcdefg")]
    public async void Get_WhenWrongEncodedUrl_ReturnsBadRequest(string shortUrl)
    {
        // Arrange

        // Act
        var result = await _urlController.Get(shortUrl);

        // Assert
        Assert.IsType<NotFoundResult>(result);
    }

    [Theory]
    [InlineData("Jdk4z3P")] // Encoded id = 9
    public async void Get_WhenCorrectUrlButWrongId_ReturnsNotFound(string shortUrl)
    {
        // Arrange

        // Act
        var result = await _urlController.Get(shortUrl);

        // Assert
        Assert.IsType<NotFoundResult>(result);
    }

    [Theory]
    [InlineData("vx31Wk9")] // Encoded id = 10
    public async void Get_WhenCorrectUrl_ReturnsOkObjectResult(string shortUrl)
    {
        // Arrange

        // Act
        var result = await _urlController.Get(shortUrl);

        // Assert
        Assert.IsType<OkObjectResult>(result);

        var resultObj = result as OkObjectResult;

        Assert.Equal("https://fullUrl.com", resultObj?.Value);
    }

    [Theory]
    [InlineData("https://thisIsLongUrl.com")]
    public async void Save_WhenCorrectUrl_ButErrorWhileSaving_ReturnsInternalServerErrorResult(string url)
    {
        // Arrange
        _context.Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(0); // Object not saved

        // Act
        var result = await _urlController.Save(url);

        // Assert        
        var resultObj = result as StatusCodeResult;

        Assert.Equal(500, resultObj?.StatusCode);
    }

    [Theory]
    [InlineData("https://thisIsLongUrl.com")]
    public async void Save_WhenCorrectUrl_ReturnsOkObjectResult(string url)
    {
        // Arrange
        _context.Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(1); // Object saved

        // Act
        var result = await _urlController.Save(url);

        // Assert        
        var resultObj = result as ObjectResult;

        Assert.Equal(201, resultObj?.StatusCode);
        Assert.Equal(_hashids.Encode(0), resultObj?.Value); // Encoded id = 0
    }

    private static List<Url> GetUrls()
    {
        List<Url> urls = new()
        {
            new()
            {
                Id = 10,
                FullUrl = "https://fullUrl.com"
            }
        };

        return urls;
    }
}