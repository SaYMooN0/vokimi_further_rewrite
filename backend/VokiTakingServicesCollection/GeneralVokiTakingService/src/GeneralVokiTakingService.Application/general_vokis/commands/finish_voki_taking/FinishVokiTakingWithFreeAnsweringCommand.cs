using GeneralVokiTakingService.Domain.common;
using GeneralVokiTakingService.Domain.common.interfaces.repositories;
using GeneralVokiTakingService.Domain.common.interfaces.repositories.taking_sessions;
using GeneralVokiTakingService.Domain.general_voki_aggregate;
using GeneralVokiTakingService.Domain.voki_taken_record_aggregate;
using GeneralVokiTakingService.Domain.voki_taking_session_aggregate;
using SharedKernel;
using SharedKernel.auth;

namespace GeneralVokiTakingService.Application.general_vokis.commands.finish_voki_taking;

public sealed record FinishVokiTakingWithFreeAnsweringCommand(
    VokiId VokiId,
    VokiTakingSessionId SessionId,
    Dictionary<GeneralVokiQuestionId, ImmutableHashSet<GeneralVokiAnswerId>> ChosenAnswers,
    DateTime ClientStartTime,
    DateTime ServerStartTime,
    DateTime ClientFinishTime
) : ICommand<FinishVokiTakingCommandsResult>;

internal sealed class FinishVokiTakingWithFreeAnsweringCommandHandler :
    ICommandHandler<FinishVokiTakingWithFreeAnsweringCommand, FinishVokiTakingCommandsResult>
{
    private readonly IGeneralVokisRepository _generalVokisRepository;
    private readonly IUserContext _userContext;
    private readonly IDateTimeProvider _dateTimeProvider;
    private readonly ISessionsWithFreeAnsweringRepository _sessionsWithFreeAnsweringRepository;
    private readonly IGeneralVokiTakenRecordsRepository _generalVokiTakenRecordsRepository;

    public FinishVokiTakingWithFreeAnsweringCommandHandler(
        IGeneralVokisRepository generalVokisRepository,
        IUserContext userContext,
        IDateTimeProvider dateTimeProvider,
        ISessionsWithFreeAnsweringRepository sessionsWithFreeAnsweringRepository,
        IGeneralVokiTakenRecordsRepository generalVokiTakenRecordsRepository
    ) {
        _generalVokisRepository = generalVokisRepository;
        _userContext = userContext;
        _dateTimeProvider = dateTimeProvider;
        _sessionsWithFreeAnsweringRepository = sessionsWithFreeAnsweringRepository;
        _generalVokiTakenRecordsRepository = generalVokiTakenRecordsRepository;
    }

    public async Task<ErrOr<FinishVokiTakingCommandsResult>> Handle(FinishVokiTakingWithFreeAnsweringCommand command,
        CancellationToken ct) {
        GeneralVoki? voki = await _generalVokisRepository.GetWithQuestionAnswersAsNoTracking(command.VokiId);
        if (voki is null) {
            return ErrFactory.NotFound.Voki("Cannot finish voki taking because requested Voki does not exist");
        }

        SessionWithFreeAnswering? session = await _sessionsWithFreeAnsweringRepository.GetById(command.SessionId);
        if (session is null) {
            return ErrFactory.NotFound.Common("Could not finish voki taking because taking session was not started");
        }


        if (
            session.ValidateTime(
                currentTime: _dateTimeProvider.UtcNow, clientStartTime: command.ClientStartTime,
                providedServerStartTime: command.ServerStartTime, clientFinishTime: command.ClientFinishTime
            ).IsErr(out var err)
        ) {
            return err;
        }

        
        if (session.ValidateVokiTaker(_userContext, out var vokiTakerId).IsErr(out err)) {
            return err;
        }

        if (session.ValidateChosenAnswers(command.ChosenAnswers).IsErr(out err)) {
            return err;
        }

        ErrOr<VokiResult> resOrErr = voki.GetResultByChosenAnswers(command.ChosenAnswers);
        if (resOrErr.IsErr(out err)) {
            return err;
        }

        VokiResult receivedResult = resOrErr.AsSuccess();
        GeneralVokiTakenRecord vokiTakenRecord = GeneralVokiTakenRecord.CreateNew(
            command.VokiId, vokiTakerId,
            command.ClientStartTime, command.ClientFinishTime,
            session.IsWithForceSequentialAnswering, receivedResult.Id,
            session.GatherQuestionDetails(command.ChosenAnswers)
        );
        await _generalVokiTakenRecordsRepository.Add(vokiTakenRecord);
        await _sessionsWithFreeAnsweringRepository.Delete(session);

        TimeSpan timeTaken = command.ClientFinishTime - command.ClientStartTime;
        return new FinishVokiTakingCommandsResult(receivedResult, timeTaken);
    }
}