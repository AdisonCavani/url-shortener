using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using HashidsNet;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MockQueryable.Moq;
using Moq;
using UrlService.Database;
using UrlService.Endpoints.Url;
using UrlShortener.Shared.Contracts.Requests;
using Xunit;

namespace UrlService.UnitTests.Endpoints.Url;

public class SaveTests
{
    private readonly Mock<AppDbContext> _context = new(new DbContextOptionsBuilder<AppDbContext>().Options);
    
    private readonly Save _endpoint;
    
    public SaveTests()
    {
        _endpoint = new(_context.Object, new Hashids("1234", 7));
        
        var mockSet = Enumerable.Empty<Database.Entities.Url>().AsQueryable().BuildMockDbSet();
        _context.Setup(m => m.Urls).Returns(mockSet.Object);
    }
    
    [Theory]
    [InlineData("http://test.com")]
    [InlineData("https://example.com")]
    public async Task WhenCorrectUrl_ButNotSaved_Returns500(string url)
    {
        // Arrange
        _context.Setup(x => 
                x.SaveChangesAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(0);
        
        var req = new SaveUrlRequest
        {
            Url = url
        };

        // Act
        var res = await _endpoint.HandleAsync(req);
        
        // Assert
        var resObj = Assert.IsType<StatusCodeResult>(res.Result);
        Assert.Equal(StatusCodes.Status500InternalServerError, resObj?.StatusCode);
    }

    [Theory]
    [InlineData("http://test.com")]
    [InlineData("https://example.com")]
    public async Task ReturnsOk(string url)
    {
        // Arrange
        _context.Setup(x => 
                x.SaveChangesAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(1);
        
        var req = new SaveUrlRequest
        {
            Url = url
        };

        // Act
        var res = await _endpoint.HandleAsync(req);
        
        // Assert
        var resObj = Assert.IsType<ObjectResult>(res.Result);
        Assert.Equal(StatusCodes.Status201Created, resObj?.StatusCode);
    }
}