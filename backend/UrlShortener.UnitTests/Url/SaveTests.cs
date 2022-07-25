using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using HashidsNet;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MockQueryable.Moq;
using Moq;
using UrlShortener.Api.Database;
using UrlShortener.Api.Endpoints.Url;
using UrlShortener.Shared.Contracts.Requests;
using Xunit;

namespace UrlShortener.UnitTests.Url;

public class SaveTests
{
    private readonly Mock<AppDbContext> _context = new(new DbContextOptionsBuilder<AppDbContext>().Options);
    
    private readonly Save _endpoint;
    
    public SaveTests()
    {
        _endpoint = new(_context.Object, new Hashids("1234", 7));
        
        var mockSet = Enumerable.Empty<Api.Database.Entities.Url>().AsQueryable().BuildMockDbSet();
        _context.Setup(m => m.Urls).Returns(mockSet.Object);
    }
    
    [Theory]
    [InlineData("http://test.com")]
    [InlineData("https://example.com")]
    public async Task Get_WhenCorrectUrl_ButNotSaved_Returns_500(string url)
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
        Assert.IsType<StatusCodeResult>(res.Result);

        var resObj = res.Result as StatusCodeResult;
        
        Assert.Equal(StatusCodes.Status500InternalServerError, resObj?.StatusCode);
    }

    [Theory]
    [InlineData("http://test.com")]
    [InlineData("https://example.com")]
    public async Task Get_WhenCorrectUrl_ReturnsOk(string url)
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
        Assert.IsType<ObjectResult>(res.Result);

        var resObj = res.Result as ObjectResult;
        
        Assert.Equal(StatusCodes.Status201Created, resObj?.StatusCode);
    }
}