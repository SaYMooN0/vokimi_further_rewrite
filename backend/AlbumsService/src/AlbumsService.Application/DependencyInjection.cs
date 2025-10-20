using ApplicationShared;
using Microsoft.Extensions.DependencyInjection;
namespace AlbumsService.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services) {
        services.AddApplicationMessaging(typeof(DependencyInjection));
        return services;
    }
}