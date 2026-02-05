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
    GeneralVokiQuestionContent Content,
    QuestionOrderInVokiTakingSession OrderInVokiTaking,
    ushort MinAnswersCount,
    ushort MaxAnswersCount
)
{
    public static VokiTakingQuestionData Create(VokiQuestion question, QuestionOrderInVokiTakingSession orderInVokiTaking) => new(
        question.Id,
        question.Text,
        question.ImageSet,
        question.Content,
        orderInVokiTaking,
        question.AnswersCountLimit.MinAnswers,
        question.AnswersCountLimit.MaxAnswers
    );
}