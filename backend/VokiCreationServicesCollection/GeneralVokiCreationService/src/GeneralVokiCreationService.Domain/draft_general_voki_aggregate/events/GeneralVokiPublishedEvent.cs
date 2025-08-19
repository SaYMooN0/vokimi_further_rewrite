using GeneralVokiCreationService.Domain.draft_general_voki_aggregate.answers.type_specific_data;
using GeneralVokiCreationService.Domain.draft_general_voki_aggregate.questions;
using GeneralVokiCreationService.Domain.draft_general_voki_aggregate.results;
using VokiCreationServicesLib.Domain.draft_voki_aggregate;
using VokimiStorageKeysLib.concrete_keys;
using VokimiStorageKeysLib.general_voki.result_image;

namespace GeneralVokiCreationService.Domain.draft_general_voki_aggregate.events;

public record GeneralVokiPublishedEvent(
    VokiId VokiId,
    AppUserId PrimaryAuthorId,
    VokiCoAuthorIdsSet CoAuthors,
    VokiName Name,
    VokiCoverKey Cover,
    VokiDetails Details,
    VokiTagsSet Tags,
    DateTime InitializingDate,
    DateTime PublishingDate,
    //Voki Type specific 
    QuestionDomainEventDto[] Questions,
    bool ForceSequentialAnswering,
    bool ShuffleQuestions,
    ResultDomainEventDto[] Results
) : IDomainEvent;

public record QuestionDomainEventDto(
    GeneralVokiQuestionId Id,
    VokiQuestionText Text,
    VokiQuestionImagesSet Images,
    GeneralVokiAnswerType AnswersType,
    ushort OrderInVoki,
    AnswerDomainEventDto[] Answers,
    bool ShuffleAnswers,
    QuestionAnswersCountLimit AnswersCountLimit
);

public record AnswerDomainEventDto(
    GeneralVokiAnswerId Id,
    ushort OrderInQuestion,
    BaseVokiAnswerTypeData TypeData,
    GeneralVokiResultId[] RelatedResultIds
);

public record ResultDomainEventDto(
    GeneralVokiResultId Id,
    VokiResultName Name,
    VokiResultText Text,
    GeneralVokiResultImageKey? Image
);