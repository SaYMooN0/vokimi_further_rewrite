using GeneralVokiTakingService.Domain.general_voki_aggregate;
using GeneralVokiTakingService.Domain.general_voki_aggregate.questions;
using GeneralVokiTakingService.Domain.general_voki_aggregate.questions.content;
using GeneralVokiTakingService.Domain.voki_taking_session_aggregate;
using System.Text.Json.Serialization;
using GeneralVokiTakingService.Domain.common;
using SharedKernel.common.vokis;

namespace GeneralVokiTakingService.Application.common.dtos;

public record VokiTakingData(
    VokiId VokiId,
    VokiName Name,
    bool IsWithForceSequentialAnswering,
    VokiTakingQuestionData[] Questions,
    VokiTakingSessionId SessionId,
    DateTime StartedAt,
    ushort TotalQuestionsCount
)
{
    public static VokiTakingData Create(
        GeneralVoki voki, BaseVokiTakingSession takingSession
    ) {
        var idToOrder = takingSession.QuestionsToShowOnStart();
        VokiTakingQuestionData[] questions = voki.Questions.Where(q => idToOrder
            .TryGetValue(q.Id, out _))
            .Select(q => VokiTakingQuestionData.Create(q, idToOrder[q.Id]))
            .ToArray();
        return new VokiTakingData(
            voki.Id, voki.Name, takingSession.IsWithForceSequentialAnswering, questions,
            takingSession.Id, takingSession.StartTime, (ushort)voki.Questions.Count
        );
    }
}

public record VokiTakingQuestionData(
    GeneralVokiQuestionId Id,
    string Text,
    VokiQuestionImagesSet ImagesSet,
    QuestionContentDto Content,
    QuestionOrderInVokiTakingSession OrderInVokiTaking,
    ushort MinAnswersCount,
    ushort MaxAnswersCount
)
{
    public static VokiTakingQuestionData Create(VokiQuestion question, QuestionOrderInVokiTakingSession orderInVokiTaking) => new(
        question.Id,
        question.Text,
        question.ImageSet,
        QuestionContentDto.MapFromDomain(question.Content),
        orderInVokiTaking,
        question.AnswersCountLimit.MinAnswers,
        question.AnswersCountLimit.MaxAnswers
    );
}

[JsonDerivedType(typeof(TextOnlyContentDto), typeDiscriminator: nameof(GeneralVokiQuestionContent.TextOnly))]
[JsonDerivedType(typeof(ImageOnlyContentDto), typeDiscriminator: nameof(GeneralVokiQuestionContent.ImageOnly))]
[JsonDerivedType(typeof(ImageAndTextContentDto), typeDiscriminator: nameof(GeneralVokiQuestionContent.ImageAndText))]
[JsonDerivedType(typeof(ColorOnlyContentDto), typeDiscriminator: nameof(GeneralVokiQuestionContent.ColorOnly))]
[JsonDerivedType(typeof(ColorAndTextContentDto), typeDiscriminator: nameof(GeneralVokiQuestionContent.ColorAndText))]
[JsonDerivedType(typeof(AudioOnlyContentDto), typeDiscriminator: nameof(GeneralVokiQuestionContent.AudioOnly))]
[JsonDerivedType(typeof(AudioAndTextContentDto), typeDiscriminator: nameof(GeneralVokiQuestionContent.AudioAndText))]
public abstract record QuestionContentDto
{
    public static QuestionContentDto MapFromDomain(GeneralVokiQuestionContent content) {
        return content.Match<QuestionContentDto>(
            textOnly => new TextOnlyContentDto(),
            imageOnly => new ImageOnlyContentDto(),
            imageAndText => new ImageAndTextContentDto(),
            colorOnly => new ColorOnlyContentDto(),
            colorAndText => new ColorAndTextContentDto(),
            audioOnly => new AudioOnlyContentDto(),
            audioAndText => new AudioAndTextContentDto()
        );
    }
}

public record TextOnlyContentDto : QuestionContentDto;

public record ImageOnlyContentDto : QuestionContentDto;

public record ImageAndTextContentDto : QuestionContentDto;

public record ColorOnlyContentDto : QuestionContentDto;

public record ColorAndTextContentDto : QuestionContentDto;

public record AudioOnlyContentDto : QuestionContentDto;

public record AudioAndTextContentDto : QuestionContentDto;