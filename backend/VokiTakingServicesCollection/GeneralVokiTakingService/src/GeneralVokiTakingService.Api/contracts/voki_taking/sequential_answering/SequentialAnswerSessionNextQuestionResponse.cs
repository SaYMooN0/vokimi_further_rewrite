using GeneralVokiTakingService.Application.general_vokis.commands.sequential_answering_voki_taking;
using SharedKernel.common.vokis.general_vokis;

namespace GeneralVokiTakingService.Api.contracts.voki_taking.sequential_answering;

public record class SequentialAnswerSessionNextQuestionResponse(
    string Id,
    string Text,
    string[] ImageKeys,
    double ImagesAspectRatio,
    GeneralVokiAnswerType AnswerType,
    ushort OrderInVokiTaking,
    ushort MinAnswersCount,
    ushort MaxAnswersCount,
    GeneralVokiTakingResponseAnswerData[] Answers,
    DateTime ServerShownAt
) : ICreatableResponse<AnswerQuestionInSequentialAnsweringVokiTakingCommandResult>
{
    public static ICreatableResponse<AnswerQuestionInSequentialAnsweringVokiTakingCommandResult> Create(
        AnswerQuestionInSequentialAnsweringVokiTakingCommandResult commandRes
    ) => new SequentialAnswerSessionNextQuestionResponse(
        commandRes.Id.ToString(),
        commandRes.Text,
        commandRes.ImagesSet.Keys.Select(k => k.ToString()).ToArray(),
        commandRes.ImagesSet.AspectRatio,
        commandRes.AnswerType,
        commandRes.OrderInVokiTaking,
        commandRes.MinAnswersCount,
        commandRes.MaxAnswersCount,
        commandRes.Answers.Select(GeneralVokiTakingResponseAnswerData.Create).ToArray(),
        ServerShownAt: commandRes.CurrentTime
    );
}