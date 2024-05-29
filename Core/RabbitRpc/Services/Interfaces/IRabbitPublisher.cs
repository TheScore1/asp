using RabbitMQ.Client;

namespace Core.RabbitRpc.Services.Interfaces;

public interface IRabbitPublisher
{
    public void Publish<T>(T? data, string exchange, string routeKey, string replyQueue, string correlationId);
    public T Call<T>(object data, string serviceName, string replyQueueName,
        CancellationToken cancellationToken = default);
    public void ExchangeDeclare(string exchange, string type, bool durable, bool autoDelete, IDictionary<string, object> arguments);
    public QueueDeclareOk QueueDeclare(string queue, bool durable, bool exclusive, bool autoDelete, IDictionary<string, object>? arguments);
}