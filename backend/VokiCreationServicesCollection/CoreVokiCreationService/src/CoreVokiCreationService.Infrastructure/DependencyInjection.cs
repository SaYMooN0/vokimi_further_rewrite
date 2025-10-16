﻿using Amazon.Runtime;
using Amazon.S3;
using ApplicationShared;
using CoreVokiCreationService.Application;
using CoreVokiCreationService.Application.common;
using CoreVokiCreationService.Application.common.repositories;
using CoreVokiCreationService.Infrastructure.persistence;
using CoreVokiCreationService.Infrastructure.persistence.repositories;
using CoreVokiCreationService.Infrastructure.storage;
using Infrastructure.Auth;
using InfrastructureShared.Base;
using InfrastructureShared.Base.domain_events_publisher;
using InfrastructureShared.Storage;
using MassTransit;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SharedKernel.auth;

namespace CoreVokiCreationService.Infrastructure;

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
            .AddIntegrationEventsPublisher()
            .AddS3(configuration);
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
        string dbConnectionString = configuration.GetConnectionString("CoreVokiCreationServiceDb")
                                    ?? throw new Exception("Database connection string is not provided.");
        services.AddDbContext<CoreVokiCreationDbContext>(options => {
                options.UseNpgsql(dbConnectionString);
                options.ConfigureDevelopmentExclusive(env);
            }
        );
        services.AddScoped<IAppUsersRepository, AppUsersRepository>();
        services.AddScoped<IDraftVokiRepository, DraftVokiRepository>();

        return services;
    }

    private static IServiceCollection AddS3(this IServiceCollection services, IConfiguration configuration) {
        var s3Config = configuration.GetSection("S3").Get<S3Config>();
        if (s3Config is null) {
            throw new Exception("S3 is not configured");
        }

        services.AddSingleton<IAmazonS3>(_ => new AmazonS3Client(
            new BasicAWSCredentials(s3Config.AccessKey, s3Config.SecretKey),
            new AmazonS3Config { ServiceURL = s3Config.ServiceUrl }
        ));

        services.AddSingleton(s3Config.MainBucket); //S3MainBucketConf
        services.AddScoped<IMainStorageBucket, MainStorageBucket>();

        return services;
    }
}