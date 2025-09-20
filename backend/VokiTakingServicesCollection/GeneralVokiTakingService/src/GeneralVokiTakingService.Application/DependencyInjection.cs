using Microsoft.Extensions.DependencyInjection;
using VokiTakingServicesLib.Application;

namespace GeneralVokiTakingService.Application;

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

        services.AddLibStepHandlers();
        
        return services;
    }
}