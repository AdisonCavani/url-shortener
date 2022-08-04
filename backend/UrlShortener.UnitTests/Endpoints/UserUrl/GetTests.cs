using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using HashidsNet;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moq;
using UrlShortener.Api.Database;
using UrlShortener.Api.Endpoints.UserUrl;
using UrlShortener.Shared.Contracts.Requests;
using UrlShortener.Shared.Contracts.Responses;
using Xunit;

namespace UrlShortener.UnitTests.Endpoints.UserUrl;

public class GetTests
{
    private readonly Mock<AppDbContext> _context = new(new DbContextOptionsBuilder<AppDbContext>().Options);

    private readonly Get _endpoint;

    public GetTests()
    {
        _context.Setup(m => m.Urls).Returns(UrlHelpers.GetMockSet());

        _endpoint = new(_context.Object, new Hashids("1234", 7));
    }

    [Fact]
    public async Task WhenInvalidClaims_Returns500()
    {
        // Arrange
        var req = new GetUserUrlRequest
        {
            Id = 1
        };

        // Act
        var res = await _endpoint.HandleAsync(req);

        // Assert
        Assert.IsType<StatusCodeResult>(res.Result);

        var resObj = res.Result as StatusCodeResult;

        Assert.Equal(StatusCodes.Status500InternalServerError, resObj?.StatusCode);
    }

    [Fact]
    public async Task WhenDoesNotExistInDb_ReturnsNotFound()
    {
        // Arrange
        _endpoint.ControllerContext = new()
        {
            HttpContext = UrlHelpers.GetHttpContext()
        };

        var req = new GetUserUrlRequest
        {
            Id = 144
        };

        // Act
        var res = await _endpoint.HandleAsync(req);

        // Assert
        Assert.IsType<NotFoundResult>(res.Result);
    }
    
    [Fact]
    public async Task WhenExist_ButDoesNotOwnUrl_ReturnsForbidden()
    {
        // Arrange
        _endpoint.ControllerContext = new()
        {
            HttpContext = UrlHelpers.GetHttpContext()
        };

        var req = new GetUserUrlRequest
        {
            Id = 55
        };

        // Act
        var res = await _endpoint.HandleAsync(req);

        // Assert
        Assert.IsType<ForbidResult>(res.Result);
    }
    
    [Fact]
    public async Task ReturnsOk()
    {
        // Arrange
        _endpoint.ControllerContext = new()
        {
            HttpContext = UrlHelpers.GetHttpContext()
        };

        var req = new GetUserUrlRequest
        {
            Id = 1
        };

        // Act
        var res = await _endpoint.HandleAsync(req);

        // Assert
        var resVal = Assert.IsType<OkObjectResult>(res.Result);
        var resObj = Assert.IsType<GetUserUrlResponse>(resVal.Value);

        var expected = _context.Object.Urls.FirstOrDefault(x => x.Id == req.Id);

        expected.Should().BeEquivalentTo(resObj, o =>
            o.Excluding(si => si.ShortUrl));
    }
}