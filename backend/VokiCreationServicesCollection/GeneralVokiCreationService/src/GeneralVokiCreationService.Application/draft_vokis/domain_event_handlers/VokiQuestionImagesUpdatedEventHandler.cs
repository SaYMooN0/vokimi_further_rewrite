using GeneralVokiCreationService.Domain.draft_general_voki_aggregate.events;

namespace GeneralVokiCreationService.Application.draft_vokis.domain_event_handlers;

internal class VokiQuestionImagesUpdatedEventHandler : IDomainEventHandler<VokiQuestionImagesUpdatedEvent>
{
    private readonly IMainStorageBucket _mainStorageBucket;

    public VokiQuestionImagesUpdatedEventHandler(IMainStorageBucket mainStorageBucket) {
        _mainStorageBucket = mainStorageBucket;
    }

    public async Task Handle(VokiQuestionImagesUpdatedEvent e, CancellationToken ct) {
        var deleteRes = await _mainStorageBucket.DeleteUnusedQuestionImages(
            e.VokiId,
            e.QuestionId,
            usedKeys: e.NewImages.Keys
        );
        UnexpectedBehaviourException.ThrowIfErr(deleteRes);
    }
}