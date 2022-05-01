using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Moq;
using UrlShortener.Controllers.V1;
using UrlShortener.Models.App;
using UrlShortener.Models.Responses;
using Xunit;

namespace UrlShortener.UnitTests;

public class ConfigControllerTests
{
    // Dependencies
    private readonly Mock<IOptionsSnapshot<AuthSettings>> _authSettings = new();
    private readonly Mock<IOptionsSnapshot<AppSettings>> _appSettings = new();

    // Controller
    private readonly ConfigController _controller;

    public ConfigControllerTests()
    {
        _controller = new ConfigController();

        // Mock
        _authSettings.Setup(m => m.Value).Returns(GetAuthSettings());
        _appSettings.Setup(m => m.Value).Returns(GetAppSettings());

    }

    [Fact]
    public void Config_ReturnsOkObjectResult()
    {
        // Arrange
        SettingsDto expected = new()
        {
            AppSettings = _appSettings.Object.Value,
            AuthSettings = _authSettings.Object.Value
        };

        // Act
        var result = _controller.Config(_appSettings.Object, _authSettings.Object);

        // Assert
        var resultObj = Assert.IsType<OkObjectResult>(result);
        expected.Should().BeEquivalentTo(resultObj.Value);
    }

    private static AuthSettings GetAuthSettings()
    {
        return new()
        {
            JwtKey = "PRIVATE_KEY_DO_NOT_SHARE",
            JwtExpireMinutes = 15,
            JwtIssuer = "localhost"
        };
    }


    private static AppSettings GetAppSettings()
    {
        return new()
        {
            HashidsSalt = "salt",
            RedisConnection = "localhost:6379",
            SQLConnection = "Server=.; Database=efcore; MultipleActiveResultSets=true"
        };
    }
}