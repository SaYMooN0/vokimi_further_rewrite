using SharedKernel.common;

namespace SharedKernel.integration_events.voki_publishing;

public record class BaseVokiPublishedIntegrationEvent(
    VokiId VokiId,
    AppUserId PrimaryAuthorId,
    AppUserId[] CoAuthors,
    string Name,
    string Cover,
    string Description,
    bool IsAgeRestricted,
    Language Language,
    VokiTagId[] Tags,
    DateTime InitializingDate,
    DateTime PublishingDate
) : IIntegrationEvent;