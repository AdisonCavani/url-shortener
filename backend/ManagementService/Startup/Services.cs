using ManagementService.Database;
using ManagementService.Services;

namespace ManagementService.Startup;

public static class Services
{
    public static void RegisterServices(this IServiceCollection services)
    {
        // TODO: add RabbitMq health check
        services.AddHealthChecks()
            .AddDbContextCheck<AppDbContext>();

        services.AddHostedService<MessageBusSubscriber>();
        services.AddScoped<IDetailsRepository, DetailsRepository>();
    }
}