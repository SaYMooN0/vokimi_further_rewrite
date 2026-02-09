using ApplicationShared;
using GeneralVokiTakingService.Application.common.repositories;
using GeneralVokiTakingService.Application.common.repositories.taking_sessions;
using GeneralVokiTakingService.Application.dtos;
using GeneralVokiTakingService.Domain.common;
using GeneralVokiTakingService.Domain.general_voki_aggregate;
using GeneralVokiTakingService.Domain.voki_taking_session_aggregate;
using SharedKernel;
using SharedKernel.common.vokis;
using SharedKernel.user_ctx;

namespace GeneralVokiTakingService.Application.general_vokis.commands;

public sealed record StartVokiTakingCommand(
    VokiId VokiId,
    bool TerminateExistingUnfinishedSession
) :
    ICommand<IStartVokiTakingCommandResult>;

internal sealed class StartVokiTakingCommandHandler : ICommandHandler<StartVokiTakingCommand, IStartVokiTakingCommandResult>
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


    public async Task<ErrOr<IStartVokiTakingCommandResult>> Handle(StartVokiTakingCommand command, CancellationToken ct) {
        IUserCtx currentTaker = _userCtxProvider.Current;
        if (currentTaker.IsAuthenticated(out var aUserCtx)) {
            var startedSession = await _baseTakingSessionsRepository.GetForVokiAndUser(command.VokiId, aUserCtx, ct);
            if (startedSession is not null) {
                if (command.TerminateExistingUnfinishedSession) {
                    await _baseTakingSessionsRepository.Delete(startedSession, ct);
                }
                else {
                    return StartVokiTakingCommandUnfinishedSessionExistsResult.Create(startedSession);
                }
            }
        }

        GeneralVoki? voki = await _generalVokisRepository.GetWithQuestions(command.VokiId, ct);
        if (voki is null) {
            return ErrFactory.NotFound.Voki(
                "Requested Voki was not found", $"Voki with id: {command.VokiId} does not exist"
            );
        }

        ErrOr<BaseVokiTakingSession> startRes = voki.StartTaking(currentTaker, _dateTimeProvider.UtcNow);
        if (startRes.IsErr(out var err)) {
            return err;
        }

        var takingSession = startRes.AsSuccess();
        await _baseTakingSessionsRepository.Add(takingSession, ct);
        return SuccessStartVokiTakingCommandResult.Create(voki, takingSession);
    }
}

public interface IStartVokiTakingCommandResult;

public record StartVokiTakingCommandUnfinishedSessionExistsResult(
    SavedUnfinishedVokiTakingSessionDto SessionData
) : IStartVokiTakingCommandResult
{
    public static StartVokiTakingCommandUnfinishedSessionExistsResult Create(BaseVokiTakingSession takingSession) => new(
        SavedUnfinishedVokiTakingSessionDto.Create(takingSession)
    );
}

public record SuccessStartVokiTakingCommandResult(
    CurrentVokiTakingSessionDto SessionData
) : IStartVokiTakingCommandResult
{
    public static SuccessStartVokiTakingCommandResult Create(GeneralVoki voki, BaseVokiTakingSession takingSession) => new(
        CurrentVokiTakingSessionDto.Create(
            vokiName: voki.Name,
            vokiQuestionsById: voki.Questions.ToDictionary(q => q.Id, q => q),
            takingSession,
            questionsToShow: takingSession.QuestionsToShowOnStart()
        )
    );
}