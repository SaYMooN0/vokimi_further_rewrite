using ApplicationShared;
using ApplicationShared.messaging;
using Microsoft.Extensions.DependencyInjection;
using VokiCreationServicesLib.Application.pipeline_behaviors;

namespace VokiCreationServicesLib.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddLibMessaging(this IServiceCollection services) {
        
        services.TryDecorate(typeof(IQueryHandler<,>), typeof(VokiAccessValidationStepHandler.QueryHandler<,>));
        services.TryDecorate(typeof(ICommandHandler<,>), typeof(VokiAccessValidationStepHandler.CommandHandler<,>));
        services.TryDecorate(typeof(ICommandHandler<>), typeof(VokiAccessValidationStepHandler.CommandBaseHandler<>));
        
        return services.AddApplicationMessaging(typeof(DependencyInjection));
    }
}