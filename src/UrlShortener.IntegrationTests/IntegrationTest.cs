using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using UrlShortener.Contracts.V1;
using UrlShortener.Models.App;
using UrlShortener.Models.Requests;

namespace UrlShortener.IntegrationTests;

public class IntegrationTest : IDisposable
{
    protected readonly HttpClient TestClient;
    private readonly IServiceProvider _serviceProvider;
    private readonly string _baseUrl;
    private readonly string _apiVersion;

    protected IntegrationTest(string baseUrl, string apiVersion)
    {
        _baseUrl = baseUrl;
        _apiVersion = apiVersion;

        var appFactory = new WebApplicationFactory<Startup>().WithWebHostBuilder(builder =>
        {
            builder.ConfigureServices(services =>
            {
                services.RemoveAll(typeof(DbContextOptions<AppDbContext>));

                services.AddDbContext<AppDbContext>(options =>
                {
                    options.UseInMemoryDatabase("InMemoryDbForTesting");
                });
            });
        });

        _serviceProvider = appFactory.Services;
        TestClient = appFactory.CreateClient();
    }

    protected async Task AuthenticateAsync()
    {
        TestClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", await GetJwtAsync());
    }

    private async Task<string> GetJwtAsync()
    {
        var register = await TestClient.PostAsJsonAsync(GetRoute(ApiRoutes.Account.Register), new RegisterUserDto()
        {
            Email = "test@email.com",
            Password = "password"
        });

        var login = await TestClient.PostAsJsonAsync(GetRoute(ApiRoutes.Account.Login), new LoginDto()
        {
            Email = "test@email.com",
            Password = "password"
        });

        var token = await login.Content.ReadAsStringAsync();

        return token;
    }

    public void Dispose()
    {
        // using var serviceScope = _serviceProvider.CreateScope();
        // var context = serviceScope.ServiceProvider.GetService<AppDbContext>();
        // context?.Database.EnsureDeleted();
    }

    // Api route resolver
    protected string GetRoute(string route) => _baseUrl + route.Replace("v{version:apiVersion}", _apiVersion);
}