using CoreVokiCreationService.Domain.draft_voki_aggregate.events;
using SharedKernel.common.vokis;
using SharedKernel.integration_events;
using SharedKernel.integration_events.draft_voki_initialized;

namespace CoreVokiCreationService.Infrastructure.integration_events;

internal class DomainToIntegrationEventsHandler : BaseDomainToIntegrationEventsHandler,
    IDomainEventHandler<NewDraftVokiInitializedEvent>
// and all other domain events that need to be published as integration events
{
    private readonly IIntegrationEventPublisher _integrationEventPublisher;

    public DomainToIntegrationEventsHandler(IIntegrationEventPublisher integrationEventPublisher) {
        _integrationEventPublisher = integrationEventPublisher;
    }

    public async Task Handle(NewDraftVokiInitializedEvent e, CancellationToken ct) {
        IIntegrationEvent integrationEvent = e.VokiType.Match<IIntegrationEvent>(
            onGeneral: () => new GeneralDraftVokiInitializedIntegrationEvent(
                e.VokiId, e.PrimaryCreatorId, e.VokiName, e.VokiCoverPath, e.CreationDate
            ),
            onTierList: () => new TierListDraftVokiInitializedIntegrationEvent(
                e.VokiId, e.PrimaryCreatorId, e.VokiName, e.VokiCoverPath, e.CreationDate
            ),
            onScoring: () => new ScoringDraftVokiInitializedIntegrationEvent(
                e.VokiId, e.PrimaryCreatorId, e.VokiName, e.VokiCoverPath, e.CreationDate
            )
        );
        await _integrationEventPublisher.Publish(integrationEvent, ct);
    }
}