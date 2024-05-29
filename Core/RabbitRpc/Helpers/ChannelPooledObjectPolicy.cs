using Microsoft.Extensions.ObjectPool;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;

namespace Core.RabbitRpc.Helpers;

public class ChannelPooledObjectPolicy : IPooledObjectPolicy<IModel>
{
    private readonly RabbitConnectionOptions _connectionOptions;
    private readonly IConnection _connection;

    public ChannelPooledObjectPolicy(IOptions<RabbitConnectionOptions> optionsAccs)
        : this(optionsAccs.Value)
    { }

    public ChannelPooledObjectPolicy(RabbitConnectionOptions connectionOptions)
    {
        this._connectionOptions = connectionOptions;
        this._connection = GetConnection();
    }

    private IConnection GetConnection()
    {
        var factory = new ConnectionFactory
        {
            HostName = this._connectionOptions.HostName,
            UserName = this._connectionOptions.UserName,
            Password = this._connectionOptions.Password,
            Port = this._connectionOptions.Port,
            VirtualHost = this._connectionOptions.VHost,
        };
        return factory.CreateConnection();
    }

    public IModel Create()
    {
        return this._connection.CreateModel();
    }

    public bool Return(IModel obj)
    {
        if (obj.IsOpen)
        {
            return true;
        }
        else
        {
            obj?.Dispose();
            return false;
        }
    }
}