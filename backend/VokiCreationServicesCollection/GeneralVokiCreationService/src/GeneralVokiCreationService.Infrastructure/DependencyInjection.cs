using Amazon.Runtime;
using Amazon.S3;
using ApplicationShared;
using GeneralVokiCreationService.Application.common;
using GeneralVokiCreationService.Application.common.repositories;
using GeneralVokiCreationService.Infrastructure.persistence;
using GeneralVokiCreationService.Infrastructure.persistence.repositories;
using GeneralVokiCreationService.Infrastructure.storage;
using InfrastructureShared.Auth;
using InfrastructureShared.Base;
using InfrastructureShared.Base.domain_events_publisher;
using InfrastructureShared.Storage;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using VokiCreationServicesLib.Application.repositories;

namespace GeneralVokiCreationService.Infrastructure;

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
            .AddS3(configuration)
            .AddAuth(configuration)
            .AddMassTransitWithIntegrationEventHandlers(configuration, typeof(Application.DependencyInjection).Assembly);
    }

    private static IServiceCollection AddDefaultServices(this IServiceCollection services) => services
        .AddDateTimeProvider()
        .AddDomainEventsPublisher()
        .AddScoped<IIntegrationEventPublisher, IntegrationEventPublisher>();



    private static IServiceCollection AddPersistence(
        this IServiceCollection services,
        IConfiguration configuration,
        IWebHostEnvironment env
    ) {
        string dbConnectionString = configuration.GetConnectionString("GeneralVokiCreationServiceDb")
                                    ?? throw new Exception("Database connection string is not provided.");
        services.AddDbContext<GeneralVokiCreationDbContext>(options => {
                options.UseNpgsql(dbConnectionString);
                options.ConfigureDevelopmentExclusive(env);
            }
        );

        services.AddScoped<IDraftGeneralVokisRepository, DraftGeneralVokisRepository>();
        services.AddScoped<IDraftVokiRepository, DraftGeneralVokisRepository>();

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