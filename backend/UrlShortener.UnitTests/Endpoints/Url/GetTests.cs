using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HashidsNet;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moq;
using UrlShortener.Api.Database;
using UrlShortener.Api.Endpoints.Url;
using UrlShortener.Api.Services;
using UrlShortener.Shared.Contracts.Requests;
using Xunit;
using MockQueryable.Moq;

namespace UrlShortener.UnitTests.Endpoints.Url;

public class GetTests
{
    private readonly IHashids _hashids;
    private readonly Mock<AppDbContext> _context = new(new DbContextOptionsBuilder<AppDbContext>().Options);
    
    private readonly Get _endpoint;
    
    public GetTests()
    {
        _hashids = new Hashids("1234", 7);

        var mockSet = GetMockSet().AsQueryable().BuildMockDbSet();
        _context.Setup(m => m.Urls).Returns(mockSet.Object);

        _endpoint = new(_hashids, new UrlService(_context.Object));
    }

    [Theory]
    [InlineData("abcdefg")]
    [InlineData("1234567")]
    public async Task Get_WhenIncorrectId_ReturnsBadRequest(string id)
    {
        // Arrange
        var req = new GetUrlRequest
        {
            Id = id
        };

        // Act
        var res = await _endpoint.HandleAsync(req);
        
        // Assert
        Assert.IsType<BadRequestResult>(res.Result);
    }
    
    [Theory]
    [InlineData(3)]
    [InlineData(4)]
    public async Task Get_WhenCorrectId_ButNotInDb_ReturnsNotFound(long id)
    {
        // Arrange
        var req = new GetUrlRequest
        {
            Id = _hashids.EncodeLong(id)
        };

        // Act
        var res = await _endpoint.HandleAsync(req);
        
        // Assert
        Assert.IsType<NotFoundResult>(res.Result);
    }
    
    [Theory]
    [InlineData(1)]
    [InlineData(2)]
    public async Task Get_WhenCorrectId_ReturnsOk(long id)
    {
        // Arrange
        var req = new GetUrlRequest
        {
            Id = _hashids.EncodeLong(id)
        };

        // Act
        var res = await _endpoint.HandleAsync(req);
        
        // Assert
        Assert.IsType<OkObjectResult>(res.Result);
    }

    private static List<Api.Database.Entities.Url> GetMockSet()
    {
        List<Api.Database.Entities.Url> urls = new()
        {
            new()
            {
                Id = 1,
                FullUrl = "https://test.com"
            },
            new()
            {
                Id = 2,
                FullUrl = "http://example.com"
            }
        };

        return urls;
    }
}