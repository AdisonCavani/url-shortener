using Microsoft.EntityFrameworkCore;

namespace ManagementService.Database;

public static class Seeder
{
    public static async Task SeedDataAsync(this WebApplication app)
    {
        await using var scope = app.Services.CreateAsyncScope();
        var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();

        if (context.Database.IsRelational())
            await context.Database.MigrateAsync();
    }
}