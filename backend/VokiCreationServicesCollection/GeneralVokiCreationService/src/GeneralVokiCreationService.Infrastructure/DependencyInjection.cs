using Amazon.Runtime;
using Amazon.S3;
using ApplicationShared;
using GeneralVokiCreationService.Application;
using GeneralVokiCreationService.Domain.common.interfaces.repositories;
using GeneralVokiCreationService.Infrastructure.integration_events;
using GeneralVokiCreationService.Infrastructure.persistence;
using GeneralVokiCreationService.Infrastructure.persistence.repositories;
using GeneralVokiCreationService.Infrastructure.storage;
using InfrastructureShared.auth;
using InfrastructureShared.domain_events_publisher;
using InfrastructureShared.Storage;
using MassTransit;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using SharedKernel.auth;
using VokiCreationServicesLib.Domain.repositories;

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
            .AddConfiguredMassTransit(configuration);
    }

    private static IServiceCollection AddDefaultServices(this IServiceCollection services) => services
        .AddSingleton<IDateTimeProvider, DateTimeProvider>()
        .AddTransient<IDomainEventsPublisher, DomainEventsPublisher>()
        .AddScoped<IIntegrationEventPublisher, IntegrationEventPublisher>();

    private static IServiceCollection AddAuth(this IServiceCollection services, IConfiguration configuration) {
        var jwtTokenConfig = configuration.GetSection("JwtTokenConfig").Get<JwtTokenConfig>();
        if (jwtTokenConfig is null) {
            throw new Exception("JWT token config not configured");
        }

        services.AddSingleton(jwtTokenConfig);
        services.AddScoped<ITokenParser, TokenParser>();
        return services;
    }

    private static IServiceCollection AddConfiguredMassTransit(
        this IServiceCollection services,
        IConfiguration configuration
    ) {
        var rabbitConfig = configuration.GetSection("MessageBroker");

        var host = rabbitConfig["Host"] ?? throw new ArgumentException("MessageBroker:Host is not configured");
        var username = rabbitConfig["Username"] ??
                       throw new ArgumentException("MessageBroker:Username is not configured");
        var password = rabbitConfig["Password"] ??
                       throw new ArgumentException("MessageBroker:Password is not configured");
        var retryCountStr = rabbitConfig["RetryCount"] ??
                            throw new ArgumentException("MessageBroker:RetryCount is not configured");

        var retryIntervalStr = rabbitConfig["RetryIntervalSeconds"] ??
                               throw new ArgumentException("MessageBroker:RetryIntervalSeconds is not configured");

        if (!int.TryParse(retryCountStr, out var retryCount)) {
            throw new ArgumentException("MessageBroker:RetryCount must be an integer");
        }

        if (!int.TryParse(retryIntervalStr, out var retryIntervalSeconds)) {
            throw new ArgumentException("MessageBroker:RetryIntervalSeconds must be an integer");
        }

        services.AddMassTransit(x => {
            //consumers are in application layer
            x.AddConsumers(typeof(GeneralVokiCreationService.Application.DependencyInjection).Assembly);

            x.UsingRabbitMq((context, cfg) => {
                cfg.Host(host, h => {
                    h.Username(username);
                    h.Password(password);
                });

                var serviceName = configuration["ServiceName"] ??
                                  throw new ArgumentNullException("ServiceName is not provided");
                cfg.ConfigureEndpoints(context, new KebabCaseEndpointNameFormatter(serviceName + ":"));

                cfg.UseMessageRetry(
                    r => { r.Interval(retryCount, TimeSpan.FromSeconds(retryIntervalSeconds)); }
                );
            });
        });

        return services;
    }

    private static IServiceCollection AddPersistence(
        this IServiceCollection services,
        IConfiguration configuration,
        IWebHostEnvironment env
    ) {
        string dbConnectionString = configuration.GetConnectionString("GeneralVokiCreationServiceDb")
                                    ?? throw new Exception("Database connection string is not provided.");
        services.AddDbContext<GeneralVokiCreationDbContext>(options => {
                options.UseNpgsql(dbConnectionString);
                if (env.IsDevelopment()) {
                    options.EnableDetailedErrors();
                    options.EnableSensitiveDataLogging();
                    options.ConfigureWarnings(warning => {
                        warning.Log(
                            CoreEventId.FirstWithoutOrderByAndFilterWarning,
                            CoreEventId.RowLimitingOperationWithoutOrderByWarning
                        );
                    });
                }
            }
        );

        services.AddScoped<IAppUsersRepository, AppUsersRepository>();
        services.AddScoped<IDraftGeneralVokiRepository, DraftGeneralVokiRepository>();
        services.AddScoped<IDraftVokiRepository, DraftGeneralVokiRepository>();

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

        string mainBucketName = s3Config.BucketNames["Main"] ?? throw new Exception("Main bucket is not set");
        services.AddSingleton(new MainBucketNameProvider(mainBucketName));

        services.AddScoped<IMainStorageBucket, MainStorageBucket>();

        return services;
    }
}