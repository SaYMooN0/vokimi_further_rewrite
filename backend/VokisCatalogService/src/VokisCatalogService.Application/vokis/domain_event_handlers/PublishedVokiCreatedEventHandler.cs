using VokisCatalogService.Domain.voki_aggregate.events;

namespace VokisCatalogService.Application.vokis.domain_event_handlers;

internal class PublishedVokiCreatedEventHandler : IDomainEventHandler<PublishedVokiCreatedEvent>
{
    private readonly IMainStorageBucket _mainStorageBucket;
    public PublishedVokiCreatedEventHandler(IMainStorageBucket mainStorageBucket) {
        _mainStorageBucket = mainStorageBucket;
    }

    public async Task Handle(PublishedVokiCreatedEvent e, CancellationToken ct) {
        var deleteRes = await _mainStorageBucket.DeleteDraftVokiContentAfterPublication(
            e.VokiId
        );
        UnexpectedBehaviourException.ThrowIfErr(deleteRes);
    }
}