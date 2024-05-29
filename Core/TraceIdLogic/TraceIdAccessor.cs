using Core.TraceLogic.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Serilog.Context;

namespace Core.TraceIdLogic;

public interface ITraceIdAccessor
{

}

public static class StartUpTraceId
{
    public static IServiceCollection TryAddTraceId(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddScoped<TraceIdAccessor>();
        serviceCollection
            .TryAddScoped<ITraceWriter>(provider => provider.GetRequiredService<TraceIdAccessor>());
        serviceCollection
            .TryAddScoped<ITraceReader>(provider => provider.GetRequiredService<TraceIdAccessor>());
        serviceCollection
            .TryAddScoped<ITraceIdAccessor>(provider => provider.GetRequiredService<TraceIdAccessor>());



        return serviceCollection;
    }
}

internal class TraceIdAccessor(IHttpContextAccessor httpContextAccessor) : ITraceReader, ITraceWriter, ITraceIdAccessor
{
    public string Name => "TraceId";

    private string _value = null!;

    public string GetValue()
    {
        return this._value;
    }

    public void WriteValue(string? value)
    {
        if (string.IsNullOrWhiteSpace(value))
            value = Guid.NewGuid().ToString();

        this._value = value;
        LogContext.PushProperty(this.Name, value);
    }
}