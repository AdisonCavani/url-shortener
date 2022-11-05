using ManagementService.Contracts.RabbitMq;

namespace ManagementService.Services;

public interface IMessageBusPublisher
{
    void PublishUrlCreatedEvent(UrlCreatedEvent createdEvent);
}