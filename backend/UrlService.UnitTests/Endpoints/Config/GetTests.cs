using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Moq;
using UrlService.Configuration;
using UrlService.Endpoints.Config;
using Xunit;

namespace UrlService.UnitTests.Endpoints.Config;

public class GetTests
{
    private readonly Mock<IOptionsSnapshot<AuthSettings>> _authSettings = new();
    private readonly Mock<IOptionsSnapshot<AppSettings>> _appSettings = new();
    
    private readonly Get _controller;

    public GetTests()
    {
        _authSettings.Setup(m => m.Value).Returns(GetAuthSettings());
        _appSettings.Setup(m => m.Value).Returns(GetAppSettings());
        
        _controller = new Get(_appSettings.Object, _authSettings.Object);
    }

    [Fact]
    public void ReturnsOk()
    {
        // Arrange
        var expected = new
        {
            AppSettings = _appSettings.Object.Value,
            AuthSettings = _authSettings.Object.Value
        };

        // Act
        var res = _controller.Handle();

        // Assert
        var resObj = Assert.IsType<OkObjectResult>(res);
        expected.Should().BeEquivalentTo(resObj.Value);
    }

    private static AuthSettings GetAuthSettings()
    {
        return new()
        {
            Audience = "localhost",
            Issuer = "localhost",
        };
    }


    private static AppSettings GetAppSettings()
    {
        return new()
        {
            HashidsSalt = "1234",
            RedisConnectionString = "localhost:6379",
            SqlConnectionString = "Server=.; Database=efcore; MultipleActiveResultSets=true"
        };
    }
}