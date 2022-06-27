using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Threading.Tasks;
using UrlShortener.Core.Contracts.V1;
using UrlShortener.Core.Models.Requests;
using UrlShortener.Core.Models.Responses;

namespace UrlShortener.IntegrationTests;

public static class AuthHelpers
{
    public static async Task AuthenticateAsync(this HttpClient client, RouteResolver route)
    {
        const string email = "test@email.com";
        const string password = "Password123!";

        var register = await client.PostAsJsonAsync(route.Get(ApiRoutes.Account.Register), new RegisterCredentialsDto
        {
            FirstName = "Test",
            LastName = "User",
            Email = email,
            Password = password
        });

        var login = await client.PostAsJsonAsync(route.Get(ApiRoutes.Account.Login), new LoginCredentialsDto
        {
            Email = email,
            Password = password
        });

        var json = await login.Content.ReadAsStringAsync();
        var loginDto = JsonSerializer.Deserialize<JwtTokenDto>(json);

        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", loginDto?.Token);
    }
}