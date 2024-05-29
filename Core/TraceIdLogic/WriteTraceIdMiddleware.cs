using Microsoft.AspNetCore.Http;
using Newtonsoft.Json.Serialization;
using ITraceWriter = Core.TraceLogic.Interfaces.ITraceWriter;

namespace Core.TraceIdLogic;

public sealed class WriteTraceIdMiddleware(IEnumerable<ITraceWriter> traceWriters) : IMiddleware
{
    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        foreach (var traceWriter in traceWriters)
            traceWriter.GetValue();

        await next(context);
    }
}