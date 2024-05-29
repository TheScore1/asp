using Microsoft.AspNetCore.Builder;
using Serilog;

namespace Core.Logger;

public static class DependencyInjection
{
    public static ConfigureHostBuilder AddLoggerServices(this ConfigureHostBuilder host)
    {
        host.UseSerilog((context, config) =>
        {
            config.ReadFrom.Configuration(context.Configuration);
        });

        return host;
    }
}