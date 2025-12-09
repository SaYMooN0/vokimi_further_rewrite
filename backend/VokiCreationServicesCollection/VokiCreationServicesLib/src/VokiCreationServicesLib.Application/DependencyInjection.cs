using ApplicationShared;
using ApplicationShared.messaging;
using Microsoft.Extensions.DependencyInjection;
using VokiCreationServicesLib.Application.pipeline_behaviors;

namespace VokiCreationServicesLib.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddLibMessaging(this IServiceCollection services) {
        return services.AddApplicationMessaging(typeof(DependencyInjection));
    }
    public static IServiceCollection AddLibStepHandlers(this IServiceCollection services) {

        services.TryDecorate(typeof(IQueryHandler<,>), typeof(VokiAccessValidationStepHandler.QueryHandler<,>));
        services.TryDecorate(typeof(ICommandHandler<,>), typeof(VokiAccessValidationStepHandler.CommandHandler<,>));
        services.TryDecorate(typeof(ICommandHandler<>), typeof(VokiAccessValidationStepHandler.CommandBaseHandler<>));

        services.TryDecorate(typeof(IQueryHandler<,>), typeof(VokiPrimaryAuthorValidationStepHandler.QueryHandler<,>));
        services.TryDecorate(typeof(ICommandHandler<,>), typeof(VokiPrimaryAuthorValidationStepHandler.CommandHandler<,>));
        services.TryDecorate(typeof(ICommandHandler<>), typeof(VokiPrimaryAuthorValidationStepHandler.CommandBaseHandler<>));
        
        return services;
    } 
}