using MassTransit;
using SharedKernel.integration_events;

namespace GeneralVokiTakingService.Infrastructure.integration_events;

public interface IIntegrationEventPublisher
{
    Task Publish<T>(T integrationEvent, CancellationToken ct = default) where T : class, IIntegrationEvent;
}

public class IntegrationEventPublisher : IIntegrationEventPublisher
{
    private readonly IPublishEndpoint _publish;

    public IntegrationEventPublisher(IPublishEndpoint publish) {
        _publish = publish;
    }

    public Task Publish<T>(T integrationEvent, CancellationToken ct = default) where T : class, IIntegrationEvent =>
        _publish.Publish(integrationEvent, ct);
}