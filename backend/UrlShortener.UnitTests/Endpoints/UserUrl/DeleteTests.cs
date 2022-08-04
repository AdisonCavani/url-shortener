using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moq;
using StackExchange.Redis;
using UrlShortener.Api.Database;
using UrlShortener.Api.Endpoints.UserUrl;
using UrlShortener.Shared.Contracts.Requests;
using Xunit;

namespace UrlShortener.UnitTests.Endpoints.UserUrl;

public class DeleteTests
{
    private readonly Mock<AppDbContext> _context = new(new DbContextOptionsBuilder<AppDbContext>().Options);
    private readonly Mock<IConnectionMultiplexer> _connectionMultiplexer = new();

    private readonly Delete _endpoint;

    public DeleteTests()
    {
        _context.Setup(m => m.Urls).Returns(UrlHelpers.GetMockSet());
        _connectionMultiplexer
            .Setup(_ => _.GetDatabase(It.IsAny<int>(), It.IsAny<object>()))
            .Returns(new Mock<IDatabase>().Object);

        _endpoint = new(_context.Object, _connectionMultiplexer.Object);
    }

    [Fact]
    public async Task WhenInvalidClaims_Returns500()
    {
        // Arrange
        var req = new DeleteUserUrlRequest
        {
            Id = 1
        };

        // Act
        var res = await _endpoint.HandleAsync(req);

        // Assert
        var resObj = Assert.IsType<StatusCodeResult>(res);
        Assert.Equal(StatusCodes.Status500InternalServerError, resObj.StatusCode);
    }

    [Fact]
    public async Task WhenExist_ButDoesNotOwnUrl_ReturnsForbidden()
    {
        // Arrange
        _endpoint.ControllerContext = new()
        {
            HttpContext = UrlHelpers.GetHttpContext()
        };

        var req = new DeleteUserUrlRequest
        {
            Id = 144
        };

        // Act
        var res = await _endpoint.HandleAsync(req);

        // Assert
        Assert.IsType<NotFoundResult>(res);
    }

    [Fact]
    public async Task WhenInvalidId_ReturnsNotFound()
    {
        // Arrange
        _endpoint.ControllerContext = new()
        {
            HttpContext = UrlHelpers.GetHttpContext()
        };

        var req = new DeleteUserUrlRequest
        {
            Id = 55
        };

        // Act
        var res = await _endpoint.HandleAsync(req);

        // Assert
        Assert.IsType<ForbidResult>(res);
    }

    [Fact]
    public async Task WhenCorrectId_ButNotSaved_Returns500()
    {
        // Arrange
        _context.Setup(x => x
                .SaveChangesAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(0);

        _endpoint.ControllerContext = new()
        {
            HttpContext = UrlHelpers.GetHttpContext()
        };

        var req = new DeleteUserUrlRequest
        {
            Id = 2
        };

        // Act
        var res = await _endpoint.HandleAsync(req);

        // Assert
        var resObj = Assert.IsType<StatusCodeResult>(res);
        Assert.Equal(StatusCodes.Status500InternalServerError, resObj.StatusCode);
    }

    [Fact]
    public async Task ReturnsOk()
    {
        // Arrange
        _context.Setup(x =>
                x.SaveChangesAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(1);

        _endpoint.ControllerContext = new()
        {
            HttpContext = UrlHelpers.GetHttpContext()
        };

        var req = new DeleteUserUrlRequest
        {
            Id = 2
        };

        // Act
        var res = await _endpoint.HandleAsync(req);

        // Assert
        Assert.IsType<OkResult>(res);
    }
}