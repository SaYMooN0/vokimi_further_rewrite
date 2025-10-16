using ApplicationShared;
using Microsoft.Extensions.DependencyInjection;
using VokiTakingServicesLib.Application;

namespace GeneralVokiTakingService.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services) {
        services.AddApplicationMessaging(typeof(DependencyInjection));
        services.AddLibStepHandlers();
        
        return services;
    }
}