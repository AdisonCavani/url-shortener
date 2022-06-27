using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using UrlShortener.Contracts.V1;
using UrlShortener.Models.Requests;

namespace UrlShortener.IntegrationTests;

public static class AuthHelpers
{
    public static async Task AuthenticateAsync(this HttpClient client, RouteResolver route)
    {
        const string email = "test@email.com";
        const string password = "Password123!";
        
        var register = await client.PostAsJsonAsync(route.Get(ApiRoutes.Account.Register), new RegisterUserDto()
        {
            Email = email,
            Password = password
        });

        var login = await client.PostAsJsonAsync(route.Get(ApiRoutes.Account.Login), new LoginDto()
        {
            Email = email,
            Password = password
        });

        var token = await login.Content.ReadAsStringAsync();

        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", token);
    }
}