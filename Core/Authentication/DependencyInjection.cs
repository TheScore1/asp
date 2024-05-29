using System.Runtime.Serialization;
using Microsoft.Extensions.DependencyInjection;

namespace Core.Authentication;

public static class DependencyInjection
{
    public static IServiceCollection AddAuthenticationHelper(this IServiceCollection services)
    {
        services.AddScoped<IAuthenticationHelper, AuthenticationHelper>();

        return services;
    }
}