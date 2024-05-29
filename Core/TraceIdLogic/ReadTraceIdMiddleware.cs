using Core.TraceLogic.Interfaces;
using Microsoft.AspNetCore.Http;

namespace Core.TraceIdLogic;

public class ReadTraceIdMiddleware(IEnumerable<ITraceReader> traceReaders) : IMiddleware
{
    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        foreach (var traceReader in traceReaders)
            traceReader.WriteValue(context.Request.Headers["TraceId"]);

        await next(context);
    }
}