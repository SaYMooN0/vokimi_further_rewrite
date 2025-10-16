using AlbumsService.Application.common.repositories;
using AlbumsService.Infrastructure.persistence;
using AlbumsService.Infrastructure.persistence.repositories;
using ApplicationShared;
using Infrastructure.Auth;
using InfrastructureShared.Base;
using InfrastructureShared.Base.domain_events_publisher;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace AlbumsService.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration,
        IWebHostEnvironment env
    ) {
        return services
            .AddDefaultServices()
            .AddPersistence(configuration, env)
            .AddAuth(configuration)
            .AddMassTransitWithIntegrationEventHandlers(configuration, typeof(Application.DependencyInjection).Assembly)
            .AddIntegrationEventsPublisher();
    }

    private static IServiceCollection AddDefaultServices(this IServiceCollection services) => services
        .AddDateTimeProvider()
        .AddDomainEventsPublisher()
        .AddScoped<IIntegrationEventPublisher, IntegrationEventPublisher>();

    private static IServiceCollection AddIntegrationEventsPublisher(this IServiceCollection services) {
        services.AddScoped<IIntegrationEventPublisher, IntegrationEventPublisher>();

        services
            .Scan(scan => scan.FromAssembliesOf(typeof(DependencyInjection))
                .AddClasses(classes => classes.AssignableTo(typeof(IDomainEventHandler<>)), publicOnly: false)
                .AsImplementedInterfaces()
                .WithScopedLifetime()
            );

        return services;
    }

    private static IServiceCollection AddPersistence(
        this IServiceCollection services,
        IConfiguration configuration,
        IWebHostEnvironment env
    ) {
        string dbConnectionString = configuration.GetConnectionString("AlbumsServiceDb")
                                    ?? throw new Exception("Database connection string is not provided.");
        services.AddDbContext<AlbumsDbContext>(options => {
                options.UseNpgsql(dbConnectionString);
                options.ConfigureDevelopmentExclusive(env);
            }
        );


        services.AddScoped<IAppUsersRepository, AppUsersRepository>();
        services.AddScoped<IVokisRepository, VokisRepository>();
        services.AddScoped<IVokiAlbumsRepository, VokiAlbumsRepository>();

        return services;
    }
}