using ManagementService.Contracts.RabbitMq;
using ManagementService.Options;
using Microsoft.Extensions.Options;
using ProtoBuf;
using RabbitMQ.Client;

namespace ManagementService.Services;

public class MessageBusPublisher : IMessageBusPublisher, IDisposable
{
    private readonly IConnection _connection;
    private readonly IModel _channel;

    public MessageBusPublisher(
        ILogger<MessageBusPublisher> logger,
        IOptions<ConnectionOptions> connectionOptions)
    {
        var factory = new ConnectionFactory
        {
            HostName = connectionOptions.Value.RabbitMqHost,
            Port = connectionOptions.Value.RabbitMqPort
        };

        try
        {
            _connection = factory.CreateConnection();
            _channel = _connection.CreateModel();

            _channel.ExchangeDeclare("trigger", ExchangeType.Fanout);
            _connection.ConnectionShutdown += (_, _) =>
            {
                logger.LogInformation($"{nameof(MessageBusPublisher)}: connection shutdown");
            };

            logger.LogInformation($"{nameof(MessageBusPublisher)} initialized");
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
            throw;
        }
    }

    public void PublishUrlCreatedEvent(UrlCreatedEvent createdEvent)
    {
        if (!_connection.IsOpen)
            return;

        using var stream = new MemoryStream();
        Serializer.Serialize(stream, createdEvent);
        var body = stream.ToArray();

        _channel.BasicPublish(Exchanges.UrlCreated, string.Empty, null, body);
    }

    public void Dispose()
    {
        if (_connection.IsOpen)
        {
            _connection.Dispose();
            _channel.Dispose();
        }
    }
}