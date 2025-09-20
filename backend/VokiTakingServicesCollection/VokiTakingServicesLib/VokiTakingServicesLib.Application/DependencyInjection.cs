using ApplicationShared.messaging;
using Microsoft.Extensions.DependencyInjection;
using VokiTakingServicesLib.Application.pipeline_behaviors;

namespace VokiTakingServicesLib.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddLibStepHandlers(this IServiceCollection services) {

        services.TryDecorate(typeof(IQueryHandler<,>), typeof(VokTakingAccessValidationStepHandler.QueryHandler<,>));
        services.TryDecorate(typeof(ICommandHandler<,>), typeof(VokTakingAccessValidationStepHandler.CommandHandler<,>));
        services.TryDecorate(typeof(ICommandHandler<>), typeof(VokTakingAccessValidationStepHandler.CommandBaseHandler<>));
        
        return services;
    }
}