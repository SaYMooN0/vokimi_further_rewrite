﻿using ApplicationShared;
using GeneralVokiTakingService.Application.common.repositories;
using GeneralVokiTakingService.Application.common.repositories.taking_sessions;
using GeneralVokiTakingService.Infrastructure.persistence;
using GeneralVokiTakingService.Infrastructure.persistence.repositories;
using GeneralVokiTakingService.Infrastructure.persistence.repositories.taking_sessions;
using Infrastructure.Auth;
using InfrastructureShared.Base;
using InfrastructureShared.Base.domain_events_publisher;
using MassTransit;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SharedKernel.auth;
using VokiTakingServicesLib.Application.repositories;

namespace GeneralVokiTakingService.Infrastructure;

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
        string dbConnectionString = configuration.GetConnectionString("GeneralVokiTakingServiceDb")
                                    ?? throw new Exception("Database connection string is not provided.");
        services.AddDbContext<GeneralVokiTakingDbContext>(options => {
                options.UseNpgsql(dbConnectionString);
                options.ConfigureDevelopmentExclusive(env);
            }
        );

        services.AddScoped<IAppUsersRepository, AppUsersRepository>();
        
        services.AddScoped<IGeneralVokiTakenRecordsRepository, GeneralVokiTakenRecordsRepository>();
        
        services.AddScoped<IBaseVokisRepository, GeneralVokisRepository>();
        services.AddScoped<IGeneralVokisRepository, GeneralVokisRepository>();

        services.AddScoped<IBaseTakingSessionsRepository, BaseTakingSessionsRepository>();
        services.AddScoped<ISessionsWithFreeAnsweringRepository, SessionsWithFreeAnsweringRepository>();
        services.AddScoped<ISessionsWithSequentialAnsweringRepository, SessionsWithSequentialAnsweringRepository>();

        return services;
    }
}