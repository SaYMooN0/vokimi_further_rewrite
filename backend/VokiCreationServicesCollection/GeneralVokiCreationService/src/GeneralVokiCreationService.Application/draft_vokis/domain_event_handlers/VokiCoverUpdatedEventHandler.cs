using VokiCreationServicesLib.Domain.draft_voki_aggregate.events;

namespace GeneralVokiCreationService.Application.draft_vokis.domain_event_handlers;

public class VokiCoverUpdatedEventHandler : IDomainEventHandler<VokiCoverUpdatedEvent>
{
    private readonly IMainStorageBucket _mainStorageBucket;

    public VokiCoverUpdatedEventHandler(IMainStorageBucket mainStorageBucket) {
        _mainStorageBucket = mainStorageBucket;
    }

    public async Task Handle(VokiCoverUpdatedEvent e, CancellationToken ct) {
        if (e.NewCover != e.OldCover && !e.OldCover.IsDefault()) {
            var deleteRes = await _mainStorageBucket.DeleteVokiCover(e.OldCover);
            UnexpectedBehaviourException.ThrowIfErr(deleteRes);
        }
    }
}