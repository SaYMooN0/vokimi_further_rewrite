﻿using ApplicationShared.messaging;
using ApplicationShared.messaging.pipeline_behaviors;
using Microsoft.Extensions.DependencyInjection;
using SharedKernel.domain.events;

namespace ApplicationShared;

public static class DependencyInjectionExtensions
{
    public static IServiceCollection AddApplicationMessaging(this IServiceCollection services, Type assemblyType) {
        services.Scan(scan => scan.FromAssembliesOf(assemblyType)
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
        services.TryDecorate(typeof(IQueryHandler<,>), typeof(BasicValidationStepHandler.QueryHandler<,>));
        services.TryDecorate(typeof(ICommandHandler<,>), typeof(BasicValidationStepHandler.CommandHandler<,>));
        services.TryDecorate(typeof(ICommandHandler<>), typeof(BasicValidationStepHandler.CommandBaseHandler<>));

        return services;
    }
}