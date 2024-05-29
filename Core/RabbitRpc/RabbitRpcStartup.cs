using Core.RabbitRpc.Helpers;
using Core.RabbitRpc.Services;
using Core.RabbitRpc.Services.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.ObjectPool;
using RabbitMQ.Client;

namespace Core.RabbitRpc;

public static class RabbitRpcStartup
{
    public static void AddRabbitServices(this IServiceCollection services)
    {
        services.AddSingleton<ObjectPoolProvider, DefaultObjectPoolProvider>();
        services.AddSingleton<IPooledObjectPolicy<IModel>, ChannelPooledObjectPolicy>();
        services.AddSingleton<IRabbitPublisher, RabbitPublisher>();
    }
}