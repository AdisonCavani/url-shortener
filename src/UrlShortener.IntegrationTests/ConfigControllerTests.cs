using System.Net;
using System.Threading.Tasks;
using UrlShortener.Core.Contracts.V1;
using Xunit;

namespace UrlShortener.IntegrationTests;

public class ConfigControllerTests
{
    private readonly RouteResolver _route;
    private readonly IntegrationTestWebFactory<Program> _factory;

    public ConfigControllerTests()
    {
        _route = new("https://localhost:7081/", "v1");
        _factory = new();
    }

    [Fact]
    public async Task Get_WhenBearerIsMissing_ReturnsUnauthorized()
    {
        // Arrange
        var httpClient = _factory.CreateClient();

        // Act
        var response = await httpClient.GetAsync(_route.Get(ApiRoutes.Config.Get));

        // Assert
        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }
}