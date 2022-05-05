using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using UrlShortener.Contracts.V1;
using UrlShortener.Models.Requests;

namespace UrlShortener.IntegrationTests;

public static class AuthHelpers
{
    public static async Task<HttpClient> AuthenticateAsync(this HttpClient client, RouteResolver route)
    {
        var register = await client.PostAsJsonAsync(route.Get(ApiRoutes.Account.Register), new RegisterUserDto()
        {
            Email = "test@email.com",
            Password = "password"
        });

        var login = await client.PostAsJsonAsync(route.Get(ApiRoutes.Account.Login), new LoginDto()
        {
            Email = "test@email.com",
            Password = "password"
        });

        var token = await login.Content.ReadAsStringAsync();

        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", token);

        return client;
    }
}