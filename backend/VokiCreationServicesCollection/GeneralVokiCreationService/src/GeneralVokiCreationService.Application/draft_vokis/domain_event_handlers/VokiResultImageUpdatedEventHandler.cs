using GeneralVokiCreationService.Domain.draft_general_voki_aggregate.events;

namespace GeneralVokiCreationService.Application.draft_vokis.domain_event_handlers;

internal class VokiResultImageUpdatedEventHandler : IDomainEventHandler<VokiResultImageUpdatedEvent>
{
    private readonly IMainStorageBucket _mainStorageBucket;

    public VokiResultImageUpdatedEventHandler(IMainStorageBucket mainStorageBucket) {
        _mainStorageBucket = mainStorageBucket;
    }

    public async Task Handle(VokiResultImageUpdatedEvent e, CancellationToken ct) {
        var deleteRes = await _mainStorageBucket.DeleteUnusedResultImages(
            e.VokiId,
            e.ResultId,
            currentKey: e.NewImage
        );
        UnexpectedBehaviourException.ThrowIfErr(deleteRes);
    }
}