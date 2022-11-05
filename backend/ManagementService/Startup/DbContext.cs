using ManagementService.Database;
using ManagementService.Options;
using Microsoft.EntityFrameworkCore;

namespace ManagementService.Startup;

public static class DbContext
{
    public static void ConfigureDbContext(this IServiceCollection services, ConnectionOptions connectionOptions)
    {
        services.AddDbContext<AppDbContext>(options =>
            options.UseNpgsql(connectionOptions.PostgresConnectionString, sqlOptions =>
            {
                sqlOptions.EnableRetryOnFailure(10, TimeSpan.FromSeconds(30), null);
            }));
    }
}