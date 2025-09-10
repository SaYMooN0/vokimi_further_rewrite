using GeneralVokiTakingService.Domain.common;
using GeneralVokiTakingService.Domain.common.interfaces.repositories;
using GeneralVokiTakingService.Domain.common.interfaces.repositories.taking_sessions;
using GeneralVokiTakingService.Domain.general_voki_aggregate;
using GeneralVokiTakingService.Domain.voki_taken_record_aggregate;
using GeneralVokiTakingService.Domain.voki_taking_session_aggregate;
using SharedKernel;
using SharedKernel.auth;
using VokiTakingServicesLib.Domain.common;

namespace GeneralVokiTakingService.Application.general_vokis.commands.finish_voki_taking;

public sealed record FinishVokiTakingWithFreeAnsweringCommand(
    VokiId VokiId,
    VokiTakingSessionId SessionId,
    Dictionary<GeneralVokiQuestionId, ImmutableHashSet<GeneralVokiAnswerId>> ChosenAnswers,
    DateTime ClientStartTime,
    DateTime ServerStartTime,
    DateTime ClientFinishTime
) : ICommand<VokiResult>;

internal sealed class FinishVokiTakingWithFreeAnsweringCommandHandler :
    ICommandHandler<FinishVokiTakingWithFreeAnsweringCommand, VokiResult>
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

    public async Task<ErrOr<VokiResult>> Handle(FinishVokiTakingWithFreeAnsweringCommand command, CancellationToken ct) {
        GeneralVoki? voki = await _generalVokisRepository.GetWithQuestionAnswersAsNoTracking(command.VokiId);
        if (voki is null) {
            return ErrFactory.NotFound.Voki("Cannot finish voki taking because requested Voki does not exist");
        }

        SessionWithFreeAnswering? session = await _sessionsWithFreeAnsweringRepository.GetById(command.SessionId);
        if (session is null) {
            return ErrFactory.NotFound.Common("Could not finish voki taking because taking session was not started");
        }

        AppUserId? vokiTakerId = _userContext.UserIdFromToken().IsSuccess(out var id) ? id : null;
        if (
            session.VokiTaker is not null
            && vokiTakerId is not null
            && vokiTakerId != session.VokiTaker
        ) {
            return ErrFactory.Conflict("Could not finish voki taking because it was started by another user");
        }

        vokiTakerId ??= session.VokiTaker;
        if (
            session.ValidateTime(
                currentTime: _dateTimeProvider.UtcNow, clientStartTime: command.ClientStartTime,
                providedServerStartTime: command.ServerStartTime, clientFinishTime: command.ClientFinishTime
            ).IsErr(out var err)
        ) {
            return err;
        }

        var vokiTakingRes = voki.FinishVokiTaking(
            command.ClientStartTime,
            command.ClientFinishTime,
            command.ChosenAnswers,
            vokiTakerId
        );
        if (vokiTakingRes.IsErr(out err)) {
            return err;
        }

        throw new NotImplementedException();

        var resultData = vokiTakingRes.AsSuccess();
        GeneralVokiTakenRecord vokiTakenRecord = GeneralVokiTakenRecord.CreateNew(resultData);
        await _generalVokiTakenRecordsRepository.Add(vokiTakenRecord);

        await _sessionsWithFreeAnsweringRepository.Delete(session);
    }
}