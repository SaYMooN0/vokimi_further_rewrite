using ApplicationShared;
using Microsoft.Extensions.DependencyInjection;

namespace UserProfilesService.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services) {
        services.AddApplicationMessaging(typeof(DependencyInjection));
        return services;
    }
}