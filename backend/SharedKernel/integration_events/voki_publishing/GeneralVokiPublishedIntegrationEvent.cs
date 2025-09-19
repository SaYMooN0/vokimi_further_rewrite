using SharedKernel.common;
using SharedKernel.common.vokis;
using SharedKernel.common.vokis.general_vokis;

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

public sealed record class GeneralVokiQuestionIntegrationEventDto(
    GeneralVokiQuestionId Id,
    string Text,
    string[] Images,
    Double ImagesAspectRatio,
    GeneralVokiAnswerType AnswersType,
    ushort OrderInVoki,
    GeneralVokiAnswerIntegrationEventDto[] Answers,
    bool ShuffleAnswers,
    ushort MinAnswersCount,
    ushort MaxAnswersCount
);

public sealed record class GeneralVokiAnswerIntegrationEventDto(
    GeneralVokiAnswerId Id,
    ushort OrderInQuestion,
    GeneralVokiAnswerTypeDataIntegrationEventDto TypeData,
    GeneralVokiResultId[] RelatedResultIds
);

public sealed record GeneralVokiAnswerTypeDataIntegrationEventDto(
    Dictionary<string, string> Fields
)
{
    public static class Keys
    {
        public const string Text = "Text";
        public const string Image = "Image";
        public const string Audio = "Audio";
        public const string Color = "Color";
    }
}

public sealed record class GeneralVokiResultIntegrationEventDto(
    GeneralVokiResultId Id,
    string Name,
    string Text,
    string? DraftVokiImageKey
);