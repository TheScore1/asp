using Logic.Managers.Profile;
using Microsoft.Extensions.DependencyInjection;

namespace Logic;

public static class DependencyInjection
{
    public static IServiceCollection AddLogicServices(this IServiceCollection services)
    {
        services.AddScoped<IProfileManager, ProfileManager>();

        return services;
    }
}