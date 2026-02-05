using ApplicationShared;
using GeneralVokiTakingService.Application.common.dtos;
using GeneralVokiTakingService.Application.common.repositories;
using GeneralVokiTakingService.Application.common.repositories.taking_sessions;
using GeneralVokiTakingService.Domain.common;
using GeneralVokiTakingService.Domain.general_voki_aggregate;
using GeneralVokiTakingService.Domain.voki_taking_session_aggregate;
using SharedKernel;
using SharedKernel.user_ctx;

namespace GeneralVokiTakingService.Application.general_vokis.commands;

public sealed record StartVokiTakingCommand(
    VokiId VokiId,
    bool TerminateCurrentActive
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
                if (command.TerminateCurrentActive) {
                    await _baseTakingSessionsRepository.Delete(startedSession, ct);
                }
                else {
                    return StartVokiTakingCommandActiveSessionExistsResult.Create(startedSession);
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

public record StartVokiTakingCommandActiveSessionExistsResult(
    VokiId VokiId,
    VokiTakingSessionId SessionId,
    DateTime StartedAt,
    ushort QuestionsWithSavedAnswersCount,
    ushort TotalQuestionsCount
) : IStartVokiTakingCommandResult
{
    public static StartVokiTakingCommandActiveSessionExistsResult Create(BaseVokiTakingSession takingSession) => new(
        takingSession.VokiId,
        takingSession.Id,
        takingSession.StartTime,
        takingSession.QuestionsWithSavedAnswersCount(),
        (ushort)takingSession.Questions.Length
    );
}

public record SuccessStartVokiTakingCommandResult(
    VokiTakingData Data
) : IStartVokiTakingCommandResult
{
    public static SuccessStartVokiTakingCommandResult Create(GeneralVoki voki, BaseVokiTakingSession takingSession) => new(
        VokiTakingData.Create(voki, takingSession)
    );
}