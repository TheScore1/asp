using Core.HttpLogic.Services;
using Core.HttpLogic.Services.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Core.HttpLogic;

/// <summary>
/// ����������� � DI �������� ��� HTTP-����������
/// </summary>
public static class HttpServiceStartup
{
    /// <summary>
    /// ���������� ������� ��� ������������� �������� �� HTTP
    /// </summary>
    public static IServiceCollection AddHttpRequestService(this IServiceCollection services)
    {
        services
            .AddHttpContextAccessor()
            .AddHttpClient()
            .AddTransient<IHttpConnectionService, HttpConnectionService>();

        services.TryAddTransient<IHttpRequestService, HttpRequestService>();

        return services;
    }
}