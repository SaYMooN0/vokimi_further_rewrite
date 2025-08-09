using GeneralVokiCreationService.Domain.draft_general_voki_aggregate.questions;

namespace GeneralVokiCreationService.Domain.draft_general_voki_aggregate.events;

public record class VokiQuestionImagesUpdatedEvent(
    VokiId VokiId,
    GeneralVokiQuestionId QuestionId,
    VokiQuestionImagesSet OldImage,
    VokiQuestionImagesSet NewImages
) : IDomainEvent;