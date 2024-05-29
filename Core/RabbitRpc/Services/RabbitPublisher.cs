using System.Text;
using Core.RabbitRpc.Helpers;
using Core.RabbitRpc.Services.Interfaces;
using Microsoft.Extensions.ObjectPool;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace Core.RabbitRpc.Services;

public class RabbitPublisher(IPooledObjectPolicy<IModel> objectPolicy, IOptions<RabbitConnectionOptions> options) : IRabbitPublisher
{
    private readonly DefaultObjectPool<IModel> _objectPool = new(objectPolicy, options.Value.MaxChannelCount);

    public void Publish<T>(T? data, string exchange, string routeKey, string replyQueue, string correlationId)
    {
        if (data == null)
        {
            return;
        }
        var bytesToSend = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(data));
        using var channel = new PoolObject<IModel>(this._objectPool);
        var properties = channel.Item.CreateBasicProperties();
        properties.Persistent = true;
        properties.ReplyTo = replyQueue;
        properties.CorrelationId = correlationId;
        channel.Item.BasicPublish(exchange, routeKey, properties, bytesToSend);
    }

    public T Call<T>(object data, string serviceName, string replyQueueName, CancellationToken cancellationToken = default)
    {
        var output = default(T);
        using var channel = new PoolObject<IModel>(this._objectPool).Item;

        var properties = channel.CreateBasicProperties();
        var correlationId = Guid.NewGuid().ToString();
        properties.CorrelationId = correlationId;
        properties.ReplyTo = replyQueueName;
        var serialized = JsonConvert.SerializeObject(data);
        var messageBytes = Encoding.UTF8.GetBytes(serialized);

        channel.BasicPublish(exchange: string.Empty,
            routingKey: serviceName,
            basicProperties: properties,
            body: messageBytes);


        var consumer = new EventingBasicConsumer(channel);
        consumer.Received += (ch, ea) =>
        {
            var props = ea.BasicProperties;
            if (props.CorrelationId == correlationId)
            {
                var response = Encoding.UTF8.GetString(ea.Body.ToArray());
                output = JsonConvert.DeserializeObject<T>(response);
            }
            channel.BasicAck(ea.DeliveryTag, false);
        };

        channel.BasicConsume(replyQueueName, false, consumer);

        while (output is null)
        {
            Thread.Sleep(100);
        }

        return output;
    }

    public void ExchangeDeclare(string exchange, string type, bool durable, bool autoDelete, IDictionary<string, object> arguments)
    {
        using var channel = new PoolObject<IModel>(this._objectPool);
        channel.Item.ExchangeDeclare(exchange, type, durable, autoDelete, arguments);
    }

    public QueueDeclareOk QueueDeclare(string queue, bool durable, bool exclusive, bool autoDelete, IDictionary<string, object> arguments)
    {
        using var channel = new PoolObject<IModel>(this._objectPool);
        return channel.Item.QueueDeclare(queue, durable, exclusive, autoDelete, arguments);
    }

}