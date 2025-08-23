using ApplicationShared;
using GeneralVokiTakingService.Domain.general_voki_aggregate.events;
using SharedKernel.integration_events.voki_content_saved;

namespace GeneralVokiTakingService.Application;

internal class DomainToIntegrationEventsHandler : IDomainToIntegrationEventsHandler,
    IDomainEventHandler<PublishedVokiCreatedEvent>

// and all other domain events that need to be published as integration events
{
    private readonly IIntegrationEventPublisher _integrationEventPublisher;

    public DomainToIntegrationEventsHandler(IIntegrationEventPublisher integrationEventPublisher) {
        _integrationEventPublisher = integrationEventPublisher;
    }

    public async Task Handle(PublishedVokiCreatedEvent e, CancellationToken ct) =>
        await _integrationEventPublisher.Publish(new GeneralVokiContentSavedIntegrationEvent(
            e.VokiId, e.VokiContentKeys.Select(k => k.ToString()).ToArray()
        ), ct);
}