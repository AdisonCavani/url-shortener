using UrlService.Contracts.RabbitMq;

namespace UrlService.Services;

public interface IMessageBusPublisher
{
    void PublishUrlCreatedEvent(UrlCreatedEvent createdEvent);
}