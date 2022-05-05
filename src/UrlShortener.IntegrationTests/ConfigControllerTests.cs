using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using UrlShortener.Contracts.V1;
using UrlShortener.Models.Responses;
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

    [Fact]
    public async Task Get_WhenLoggedIn_ReturnsOk()
    {
        // Arrange
        var httpClient = _factory.CreateClient();
        await httpClient.AuthenticateAsync(_route);

        // Act
        var response = await httpClient.GetAsync(_route.Get(ApiRoutes.Config.Get));

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var responseObj = await response.Content.ReadAsAsync<SettingsDto>();
        Assert.NotNull(responseObj);
    }
}