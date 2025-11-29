using ApplicationShared;
using CoreVokiCreationService.Application.pipeline_behaviors;
using Microsoft.Extensions.DependencyInjection;

namespace CoreVokiCreationService.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services) {
        services.AddApplicationMessaging(typeof(DependencyInjection));
        services.AddStepHandlers();

        return services;
    }

    private static IServiceCollection AddStepHandlers(this IServiceCollection services) {
        // @formatter:off

        services.TryDecorate(typeof(IQueryHandler<,>), typeof(VokiAccessValidationStepHandler.QueryHandler<,>));
        services.TryDecorate(typeof(ICommandHandler<,>), typeof(VokiAccessValidationStepHandler.CommandHandler<,>));
        services.TryDecorate(typeof(ICommandHandler<>), typeof(VokiAccessValidationStepHandler.CommandBaseHandler<>));

        services.TryDecorate(typeof(IQueryHandler<,>), typeof(VokiPrimaryAuthorValidationStepHandler.QueryHandler<,>));
        services.TryDecorate(typeof(ICommandHandler<,>),typeof(VokiPrimaryAuthorValidationStepHandler.CommandHandler<,>));
        services.TryDecorate(typeof(ICommandHandler<>), typeof(VokiPrimaryAuthorValidationStepHandler.CommandBaseHandler<>));
        
        // @formatter:on


        return services;
    }
}