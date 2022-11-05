using ManagementService.Contracts.RabbitMq;

namespace ManagementService.Services;

public interface IDetailsRepository
{
    Task<bool> CreateAsync(UrlCreatedEvent createdEvent);
}