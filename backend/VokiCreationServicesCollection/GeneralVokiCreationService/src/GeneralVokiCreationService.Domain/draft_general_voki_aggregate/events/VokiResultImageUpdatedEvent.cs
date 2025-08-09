using VokimiStorageKeysLib.draft_general_voki.result_image;

namespace GeneralVokiCreationService.Domain.draft_general_voki_aggregate.events;

public record class VokiResultImageUpdatedEvent(
    VokiId VokiId,
    GeneralVokiResultId ResultId,
    DraftGeneralVokiResultImageKey? OldImage,
    DraftGeneralVokiResultImageKey? NewImage
) : IDomainEvent;