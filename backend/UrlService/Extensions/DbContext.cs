using Microsoft.EntityFrameworkCore;
using UrlService.Database;

namespace UrlService.Extensions;

public static class DbContext
{
    public static void ConfigureDbContext(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<AppDbContext>(options =>
            options.UseNpgsql(configuration["AppSettings:SqlConnectionString"], sqlOptions =>
            {
                sqlOptions.EnableRetryOnFailure(10, TimeSpan.FromSeconds(30), null);
            }));
    }
}