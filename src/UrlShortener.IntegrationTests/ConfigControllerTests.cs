using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using UrlShortener.Contracts.V1;
using UrlShortener.Models.Responses;
using Xunit;

namespace UrlShortener.IntegrationTests;

public class ConfigControllerTests : IntegrationTest
{
    public ConfigControllerTests() : base("https://localhost:7081/", "v1")
    {

    }

    [Fact]
    public async Task Get_WhenBearerIsMissing_ReturnsUnauthorized()
    {
        // Arrange

        // Act
        var response = await TestClient.GetAsync(GetRoute(ApiRoutes.Config.Get));

        // Assert
        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }

    [Fact]
    public async Task Get_WhenLoggedIn_ReturnsOk()
    {
        // Arrange
        await AuthenticateAsync();

        // Act
        var response = await TestClient.GetAsync(GetRoute(ApiRoutes.Config.Get));

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var responseObj = await response.Content.ReadAsAsync<SettingsDto>();
        Assert.NotNull(responseObj);
    }
}