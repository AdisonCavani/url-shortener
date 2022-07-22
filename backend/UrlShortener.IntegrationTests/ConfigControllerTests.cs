using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Testing;
using UrlShortener.Api;
using UrlShortener.Shared.Contracts;
using Xunit;

namespace UrlShortener.IntegrationTests;

public class ConfigControllerTests
{
    private readonly IntegrationTestWebFactory<Program> _factory;

    public ConfigControllerTests()
    {
        _factory = new();
    }

    [Fact]
    public async Task Get_WhenBearerIsMissing_ReturnsUnauthorized()
    {
        // Arrange
        var httpClient = _factory.CreateClient(new WebApplicationFactoryClientOptions
        {
            BaseAddress = new("https://localhost:7112")
        });

        // Act
        var response = await httpClient.GetAsync(ApiRoutes.Config.Get);

        // Assert
        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }
}