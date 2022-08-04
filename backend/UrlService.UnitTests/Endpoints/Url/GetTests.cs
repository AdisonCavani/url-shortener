using System.Threading.Tasks;
using HashidsNet;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moq;
using UrlService.Database;
using UrlService.Endpoints.Url;
using UrlShortener.Shared.Contracts.Requests;
using Xunit;

namespace UrlService.UnitTests.Endpoints.Url;

public class GetTests
{
    private readonly IHashids _hashids;
    private readonly Mock<AppDbContext> _context = new(new DbContextOptionsBuilder<AppDbContext>().Options);
    
    private readonly Get _endpoint;
    
    public GetTests()
    {
        _hashids = new Hashids("1234", 7);
        
        _context.Setup(m => m.Urls).Returns(UrlHelpers.GetMockSet());

        _endpoint = new(_hashids, new Services.UrlService(_context.Object));
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
    [InlineData(444)]
    [InlineData(144)]
    public async Task WhenCorrectId_ButNotInDb_ReturnsNotFound(long id)
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
    public async Task ReturnsOk(long id)
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
}