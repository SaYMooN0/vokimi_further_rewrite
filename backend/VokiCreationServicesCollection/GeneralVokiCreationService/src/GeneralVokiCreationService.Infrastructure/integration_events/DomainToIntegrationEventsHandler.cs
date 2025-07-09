using SharedKernel.integration_events.draft_vokis;
using VokiCreationServicesLib.Domain.draft_voki_aggregate.events;

namespace GeneralVokiCreationService.Infrastructure.integration_events;

internal class DomainToIntegrationEventsHandler : IDomainToIntegrationEventsHandler,
    IDomainEventHandler<VokiNameUpdatedEvent>

// and all other domain events that need to be published as integration events
{
    private readonly IIntegrationEventPublisher _integrationEventPublisher;

    public DomainToIntegrationEventsHandler(IIntegrationEventPublisher integrationEventPublisher) {
        _integrationEventPublisher = integrationEventPublisher;
    }

    public async Task Handle(VokiNameUpdatedEvent e, CancellationToken ct) =>
        await _integrationEventPublisher.Publish(new DraftVokiNameUpdatedIntegrationEvent(e.VokiId, e.NewName), ct);
}