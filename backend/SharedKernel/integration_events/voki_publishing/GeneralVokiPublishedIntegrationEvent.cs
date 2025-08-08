using SharedKernel.common;

namespace SharedKernel.integration_events.voki_publishing;

public record class GeneralVokiPublishedIntegrationEvent(
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
    DateTime PublishingDate,
    //Voki Type specific 
    GeneralVokiQuestionIntegrationEventDto[] Questions,
    bool ForceSequentialAnswering,
    bool ShuffleQuestions,
    GeneralVokiResultIntegrationEventDto[] Results
) : BaseVokiPublishedIntegrationEvent(
    VokiId, PrimaryAuthorId, CoAuthors,
    Name, Cover, Description,
    IsAgeRestricted, Language, Tags,
    InitializingDate, PublishingDate
);

public record class GeneralVokiResultIntegrationEventDto();

public record class GeneralVokiQuestionIntegrationEventDto();

public record class GeneralVokiAnswerIntegrationEventDto();