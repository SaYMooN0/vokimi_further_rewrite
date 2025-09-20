using CoreVokiCreationService.Application.pipeline_behaviors;
using Microsoft.Extensions.DependencyInjection;

namespace CoreVokiCreationService.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services) {
        services.Scan(scan => scan.FromAssembliesOf(typeof(DependencyInjection))
            // queries
            .AddClasses(classes => classes.AssignableTo(typeof(IQueryHandler<,>)), publicOnly: false)
            .AsImplementedInterfaces()
            .WithScopedLifetime()

            // commands with ErrOrNothing
            .AddClasses(classes => classes.AssignableTo(typeof(ICommandHandler<>)), publicOnly: false)
            .AsImplementedInterfaces()
            .WithScopedLifetime()

            // commands with ErrOr<T>
            .AddClasses(classes => classes.AssignableTo(typeof(ICommandHandler<,>)), publicOnly: false)
            .AsImplementedInterfaces()
            .WithScopedLifetime()

            // domain events
            .AddClasses(classes => classes.AssignableTo(typeof(IDomainEventHandler<>)), publicOnly: false)
            .AsImplementedInterfaces()
            .WithScopedLifetime()
        );
        
        services.AddStepHandlers();

        return services;
    }

    private static IServiceCollection AddStepHandlers(this IServiceCollection services) {
        // @formatter:off

        services.TryDecorate(typeof(IQueryHandler<,>), typeof(VokiAccessValidationStepHandler.QueryHandler<,>));
        services.TryDecorate(typeof(ICommandHandler<,>), typeof(VokiAccessValidationStepHandler.CommandHandler<,>));
        services.TryDecorate(typeof(ICommandHandler<>), typeof(VokiAccessValidationStepHandler.CommandBaseHandler<>));

        services.TryDecorate(typeof(IQueryHandler<,>),typeof(WithMultipleVokiAccessValidationStepHandler.QueryHandler<,>));
        services.TryDecorate(typeof(ICommandHandler<,>),typeof(WithMultipleVokiAccessValidationStepHandler.CommandHandler<,>));
        services.TryDecorate(typeof(ICommandHandler<>),typeof(WithMultipleVokiAccessValidationStepHandler.CommandBaseHandler<>));

        services.TryDecorate(typeof(IQueryHandler<,>), typeof(VokiPrimaryAuthorValidationStepHandler.QueryHandler<,>));
        services.TryDecorate(typeof(ICommandHandler<,>),typeof(VokiPrimaryAuthorValidationStepHandler.CommandHandler<,>));
        services.TryDecorate(typeof(ICommandHandler<>), typeof(VokiPrimaryAuthorValidationStepHandler.CommandBaseHandler<>));
        
        // @formatter:on


        return services;
    }
}