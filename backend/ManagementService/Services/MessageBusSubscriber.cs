using ManagementService.Contracts.RabbitMq;
using ManagementService.Options;
using Microsoft.Extensions.Options;
using ProtoBuf;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace ManagementService.Services;

public class MessageBusSubscriber : BackgroundService
{
    private readonly IServiceProvider _services;
    private readonly ILogger<MessageBusSubscriber> _logger;

    private readonly IConnection _connection;
    private readonly IModel _channel;
    private readonly string _queueName;

    public MessageBusSubscriber(
        IServiceProvider services,
        ILogger<MessageBusSubscriber> logger,
        IOptions<ConnectionOptions> connectionOptions)
    {
        _services = services;
        _logger = logger;

        var factory = new ConnectionFactory
        {
            HostName = connectionOptions.Value.RabbitMqHost,
            Port = connectionOptions.Value.RabbitMqPort
        };

        _connection = factory.CreateConnection();
        _channel = _connection.CreateModel();
        
        _channel.ExchangeDeclare(Exchanges.UrlCreated, ExchangeType.Fanout);
        _queueName = _channel.QueueDeclare().QueueName;
        _channel.QueueBind(_queueName, Exchanges.UrlCreated, string.Empty);
        
        _logger.LogInformation($"{nameof(MessageBusSubscriber)} initialized");
        _connection.ConnectionShutdown += OnConnectionShutdown;
    }

    protected override Task ExecuteAsync(CancellationToken ct)
    {
        ct.ThrowIfCancellationRequested();

        var consumer = new EventingBasicConsumer(_channel);
        consumer.Received += async (_, ea) =>
        {
            var message = Serializer.Deserialize<UrlCreatedEvent>(ea.Body);
            _logger.LogInformation("Message received: {@Message}", message);

            await using var scope = _services.CreateAsyncScope();

            var repository = scope.ServiceProvider.GetRequiredService<IDetailsRepository>();
            await repository.CreateAsync(message);
        };

        _channel.BasicConsume(_queueName, true, consumer);
        
        return Task.CompletedTask;
    }

    private void OnConnectionShutdown(object? sender, ShutdownEventArgs e)
    {
        _logger.LogInformation($"{nameof(MessageBusSubscriber)} connection shutdown");
    }

    public override void Dispose()
    {
        if (_channel.IsOpen)
        {
            _channel.Close();
            _connection.Close();
        }
        
        base.Dispose();
    }
}