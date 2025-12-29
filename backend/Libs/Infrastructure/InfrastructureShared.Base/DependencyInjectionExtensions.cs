using System.Reflection;
using InfrastructureShared.Base.domain_events_publisher;
using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SharedKernel;

namespace InfrastructureShared.Base;

public static class DependencyInjectionExtensions
{

    public static IServiceCollection AddMassTransitWithIntegrationEventHandlers(
        this IServiceCollection services,
        IConfiguration configuration,
        Assembly applicationLayerAssembly
    ) {
        string? serviceName = configuration["ServiceName"];
        if (serviceName is null) {
            throw new ArgumentNullException(
                $"ServiceName is not provided in the service with{applicationLayerAssembly.FullName}"
            );
        }

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
            x.AddConsumers(applicationLayerAssembly);

            x.UsingRabbitMq((context, cfg) => {
                cfg.Host(host, h => {
                    h.Username(username);
                    h.Password(password);
                });


                cfg.ConfigureEndpoints(context, new KebabCaseEndpointNameFormatter(serviceName + ":"));

                cfg.UseMessageRetry(
                    r => { r.Interval(retryCount, TimeSpan.FromSeconds(retryIntervalSeconds)); }
                );
            });
        });

        return services;
    }

    public static IServiceCollection AddDateTimeProvider(this IServiceCollection services) =>
        services.AddSingleton<IDateTimeProvider, DateTimeProvider>();
    
    public static IServiceCollection AddDomainEventsPublisher(this IServiceCollection services) =>
        services.AddTransient<IDomainEventsPublisher, DomainEventsPublisher>();
}