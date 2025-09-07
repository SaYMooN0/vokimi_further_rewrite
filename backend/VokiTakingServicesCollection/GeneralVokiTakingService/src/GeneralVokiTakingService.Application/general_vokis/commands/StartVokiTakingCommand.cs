using GeneralVokiTakingService.Domain.common;
using GeneralVokiTakingService.Domain.common.interfaces.repositories;
using GeneralVokiTakingService.Domain.general_voki_aggregate;
using GeneralVokiTakingService.Domain.general_voki_aggregate.questions;
using GeneralVokiTakingService.Domain.voki_taking_session_aggregate;
using SharedKernel;
using SharedKernel.auth;
using SharedKernel.common.vokis;

namespace GeneralVokiTakingService.Application.general_vokis.commands;

public sealed record StartVokiTakingCommand(VokiId VokiId) :
    ICommand<StartVokiTakingCommandResponse>;

internal sealed class StartVokiTakingCommandHandler :
    ICommandHandler<StartVokiTakingCommand, StartVokiTakingCommandResponse>
{
    private readonly IGeneralVokisRepository _generalVokisRepository;
    private readonly IUserContext _userContext;
    private readonly IDateTimeProvider _dateTimeProvider;

    public StartVokiTakingCommandHandler(
        IGeneralVokisRepository generalVokisRepository,
        IUserContext userContext,
        IDateTimeProvider dateTimeProvider
    ) {
        _generalVokisRepository = generalVokisRepository;
        _userContext = userContext;
        _dateTimeProvider = dateTimeProvider;
    }


    public async Task<ErrOr<StartVokiTakingCommandResponse>> Handle(StartVokiTakingCommand command, CancellationToken ct) {
        var voki = await _generalVokisRepository.GetWithQuestionAnswersAsNoTracking(command.VokiId);
        if (voki is null) {
            return ErrFactory.NotFound.Voki(
                "Requested Voki was not found", $"Voki with id: {command.VokiId} does not exist"
            );
        }

        AppUserId? vokiTaker = _userContext.UserIdFromToken().IsSuccess(out var id) ? id : null;

        BaseVokiTakingSession takingSession;
        if (voki.ForceSequentialAnswering) {
            takingSession = SessionWithSequentialAnswering.Create(
                voki.Id, vokiTaker, _dateTimeProvider.UtcNow, voki.Questions, voki.ShuffleQuestions
            );
        }
        else {
            takingSession = SessionWithFreeAnswering.Create(
                voki.Id, vokiTaker, _dateTimeProvider.UtcNow, voki.Questions, voki.ShuffleQuestions
            );
        }

        return StartVokiTakingCommandResponse.Create(voki, takingSession);
    }
}

public record StartVokiTakingCommandResponse(
    VokiId Id,
    bool ForceSequentialAnswering,
    StartVokiTakingCommandResponseQuestionData[] Questions,
    VokiTakingSessionId SessionId,
    DateTime StartedAt,
    ushort TotalQuestionsCount
)
{
    public static StartVokiTakingCommandResponse Create(
        GeneralVoki voki, BaseVokiTakingSession takingSession
    ) {
        StartVokiTakingCommandResponseQuestionData[] questions;
        var idToOrder = takingSession.QuestionIdToOrder();
        if (takingSession.IsWithForceSequentialAnswering) {
            var firstQuestionInTaking = idToOrder.MinBy(idToOrder => idToOrder.Value);
            var question = voki.Questions.FirstOrDefault(q => q.Id == firstQuestionInTaking.Key)!;
            questions = [StartVokiTakingCommandResponseQuestionData.Create(question, firstQuestionInTaking.Value)];
        }
        else {
            questions = voki.Questions
                .Select(q => StartVokiTakingCommandResponseQuestionData.Create(q, idToOrder[q.Id]))
                .ToArray();
        }

        return new(
            voki.Id,
            voki.ForceSequentialAnswering,
            questions,
            takingSession.Id,
            takingSession.StartTime,
            (ushort)voki.Questions.Count
        );
    }
}

public record StartVokiTakingCommandResponseQuestionData(
    GeneralVokiQuestionId Id,
    string Text,
    VokiQuestionImagesSet ImagesSet,
    GeneralVokiAnswerType AnswerType,
    ushort OrderInVokiTaking,
    ushort MinAnswersCount,
    ushort MaxAnswersCount,
    VokiQuestionAnswer[] Answers
)
{
    public static StartVokiTakingCommandResponseQuestionData Create(
        VokiQuestion question, ushort orderInVokiTaking
    ) => new(
        question.Id,
        question.Text,
        question.ImageSet,
        question.AnswersType,
        orderInVokiTaking,
        question.AnswersCountLimit.MinAnswers,
        question.AnswersCountLimit.MaxAnswers,
        question.Answers.ToArray()
    );
}