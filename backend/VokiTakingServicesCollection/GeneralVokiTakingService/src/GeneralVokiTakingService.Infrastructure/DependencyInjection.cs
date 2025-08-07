using GeneralVokiTakingService.Domain.common.interfaces.repositories;
using GeneralVokiTakingService.Infrastructure.integration_events;
using GeneralVokiTakingService.Infrastructure.persistence;
using GeneralVokiTakingService.Infrastructure.persistence.repositories;
using InfrastructureShared.auth;
using InfrastructureShared.domain_events_publisher;
using MassTransit;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SharedKernel.auth;

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
            .AddConfiguredMassTransit(configuration)
            .AddIntegrationEventsPublisher();
    }

    private static IServiceCollection AddDefaultServices(this IServiceCollection services) {
        services.AddSingleton<IDateTimeProvider, DateTimeProvider>();
        services.AddTransient<IDomainEventsPublisher, DomainEventsPublisher>();

        return services;
    }

    private static IServiceCollection AddAuth(this IServiceCollection services, IConfiguration configuration) {
        var jwtTokenConfig = configuration.GetSection("JwtTokenConfig").Get<JwtTokenConfig>();
        if (jwtTokenConfig is null) {
            throw new Exception("JWT token config not configured");
        }

        services.AddSingleton(jwtTokenConfig);
        services.AddScoped<ITokenParser, TokenParser>();
        return services;
    }

    private static IServiceCollection AddConfiguredMassTransit(this IServiceCollection services,
        IConfiguration configuration) {
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
            x.AddConsumers(typeof(Application.DependencyInjection).Assembly);

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
        services.AddScoped<IGeneralVokisRepository, GeneralVokisRepository>();

        return services;
    }
}