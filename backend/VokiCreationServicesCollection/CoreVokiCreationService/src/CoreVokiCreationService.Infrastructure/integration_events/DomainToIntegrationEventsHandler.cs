using CoreVokiCreationService.Domain.draft_voki_aggregate.events;
using SharedKernel.common.vokis;
using SharedKernel.integration_events.draft_vokis.new_voki_initialized;

namespace CoreVokiCreationService.Infrastructure.integration_events;

internal class DomainToIntegrationEventsHandler : IDomainToIntegrationEventsHandler,
    IDomainEventHandler<NewDraftVokiInitializedEvent>
// and all other domain events that need to be published as integration events
{
    private readonly IIntegrationEventPublisher _integrationEventPublisher;

    public DomainToIntegrationEventsHandler(IIntegrationEventPublisher integrationEventPublisher) {
        _integrationEventPublisher = integrationEventPublisher;
    }

    public async Task Handle(NewDraftVokiInitializedEvent e, CancellationToken ct) {
        await e.Type.Match(
            onGeneral: async () => await _integrationEventPublisher.Publish(
                new GeneralDraftVokiInitializedIntegrationEvent(
                    e.VokiId, e.PrimaryAuthorId, e.Name, e.Cover.Value, e.CreationDate
                ), ct),
            onTierList: async () => await _integrationEventPublisher.Publish(
                new TierListDraftVokiInitializedIntegrationEvent(), ct),
            onScoring: async () => await _integrationEventPublisher.Publish(
                new ScoringDraftVokiInitializedIntegrationEvent(), ct)
        );
    }
}