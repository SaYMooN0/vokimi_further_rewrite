using ApplicationShared;
using GeneralVokiTakingService.Application.common.repositories;
using GeneralVokiTakingService.Application.common.repositories.taking_sessions;
using GeneralVokiTakingService.Domain.common;
using GeneralVokiTakingService.Domain.general_voki_aggregate;
using GeneralVokiTakingService.Domain.general_voki_aggregate.questions;
using GeneralVokiTakingService.Domain.general_voki_aggregate.questions.content;
using GeneralVokiTakingService.Domain.voki_taking_session_aggregate;
using SharedKernel;
using SharedKernel.common.vokis;
using SharedKernel.common.vokis.general_vokis;
using SharedKernel.user_ctx;

namespace GeneralVokiTakingService.Application.general_vokis.commands;

public sealed record StartVokiTakingCommand(VokiId VokiId) :
    ICommand<StartVokiTakingCommandResponse>;

internal sealed class StartVokiTakingCommandHandler :
    ICommandHandler<StartVokiTakingCommand, StartVokiTakingCommandResponse>
{
    private readonly IGeneralVokisRepository _generalVokisRepository;
    private readonly IUserCtxProvider _userCtxProvider;
    private readonly IDateTimeProvider _dateTimeProvider;
    private readonly IBaseTakingSessionsRepository _baseTakingSessionsRepository;

    public StartVokiTakingCommandHandler(
        IGeneralVokisRepository generalVokisRepository,
        IUserCtxProvider userCtxProvider,
        IDateTimeProvider dateTimeProvider,
        IBaseTakingSessionsRepository baseTakingSessionsRepository
    ) {
        _generalVokisRepository = generalVokisRepository;
        _userCtxProvider = userCtxProvider;
        _dateTimeProvider = dateTimeProvider;
        _baseTakingSessionsRepository = baseTakingSessionsRepository;
    }


    public async Task<ErrOr<StartVokiTakingCommandResponse>> Handle(
        StartVokiTakingCommand command, CancellationToken ct
    ) {
        var vokiTakerCtx = _userCtxProvider.Current;
        var voki = await _generalVokisRepository.GetWithQuestionAnswers(command.VokiId, ct);
        if (voki is null) {
            return ErrFactory.NotFound.Voki(
                "Requested Voki was not found", $"Voki with id: {command.VokiId} does not exist"
            );
        }

        if (voki.CheckUserAccessToTake(vokiTakerCtx).IsErr(out var err)) {
            return err;
        }

        BaseVokiTakingSession takingSession;
        if (voki.ForceSequentialAnswering) {
            takingSession = SessionWithSequentialAnswering.Create(
                voki.Id, vokiTakerCtx, _dateTimeProvider.UtcNow, voki.Questions, voki.ShuffleQuestions
            );
        }
        else {
            takingSession = SessionWithFreeAnswering.Create(
                voki.Id, vokiTakerCtx, _dateTimeProvider.UtcNow, voki.Questions, voki.ShuffleQuestions
            );
        }

        await _baseTakingSessionsRepository.Add(takingSession, ct);
        return StartVokiTakingCommandResponse.Create(voki, takingSession);
    }
}

public record StartVokiTakingCommandResponse(
    VokiId Id,
    VokiName Name,
    bool ForceSequentialAnswering,
    VokiTakingQuestionData[] Questions,
    VokiTakingSessionId SessionId,
    DateTime StartedAt,
    ushort TotalQuestionsCount
)
{
    public static StartVokiTakingCommandResponse Create(
        GeneralVoki voki, BaseVokiTakingSession takingSession
    ) {
        VokiTakingQuestionData[] questions;
        var idToOrder = takingSession.QuestionIdToOrder();
        if (takingSession.IsWithForceSequentialAnswering) {
            var firstQuestionInTaking = idToOrder.MinBy(idToOrder => idToOrder.Value);
            var question = voki.Questions.FirstOrDefault(q => q.Id == firstQuestionInTaking.Key)!;
            questions = [VokiTakingQuestionData.Create(question, firstQuestionInTaking.Value)];
        }
        else {
            questions = voki.Questions
                .Select(q => VokiTakingQuestionData.Create(q, idToOrder[q.Id]))
                .ToArray();
        }

        return new StartVokiTakingCommandResponse(
            voki.Id, voki.Name, voki.ForceSequentialAnswering, questions,
            takingSession.Id, takingSession.StartTime, (ushort)voki.Questions.Count
        );
    }
}

public record VokiTakingQuestionData(
    GeneralVokiQuestionId Id,
    string Text,
    VokiQuestionImagesSet ImagesSet,
    ushort OrderInVokiTaking,
    ushort MinAnswersCount,
    ushort MaxAnswersCount
)
{
    public static VokiTakingQuestionData Create(
        VokiQuestion question, ushort orderInVokiTaking
    ) => new(
        question.Id,
        question.Text,
        question.ImageSet,
        orderInVokiTaking,
        question.AnswersCountLimit.MinAnswers,
        question.AnswersCountLimit.MaxAnswers
    );
}