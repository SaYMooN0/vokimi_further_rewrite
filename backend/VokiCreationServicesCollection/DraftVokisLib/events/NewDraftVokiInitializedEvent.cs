using SharedKernel.domain.events;

namespace DraftVokisLib.events;

public record class NewDraftVokiInitializedEvent(VokiId VokiId, AppUserId PrimaryAuthorId) : IDomainEvent;