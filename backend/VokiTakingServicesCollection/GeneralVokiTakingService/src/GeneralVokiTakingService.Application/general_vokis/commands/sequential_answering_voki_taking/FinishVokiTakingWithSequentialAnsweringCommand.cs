using ApplicationShared;
using GeneralVokiTakingService.Application.common.repositories;
using GeneralVokiTakingService.Application.common.repositories.taking_sessions;
using GeneralVokiTakingService.Domain.common;
using GeneralVokiTakingService.Domain.common.dtos;
using GeneralVokiTakingService.Domain.general_voki_aggregate;
using GeneralVokiTakingService.Domain.voki_taken_record_aggregate;
using GeneralVokiTakingService.Domain.voki_taking_session_aggregate;
using SharedKernel;
using SharedKernel.user_ctx;

namespace GeneralVokiTakingService.Application.general_vokis.commands.sequential_answering_voki_taking;

public sealed record FinishVokiTakingWithSequentialAnsweringCommand(
    VokiId VokiId,
    VokiTakingSessionId SessionId,
    ClientServerTimePairDto SessionStartTime,
    DateTime ClientSessionFinishTime,
    GeneralVokiQuestionId LastQuestionId,
    ushort LastQuestionOrderInVokiTaking,
    ImmutableHashSet<GeneralVokiAnswerId> LastQuestionChosenAnswers,
    ClientServerTimePairDto LastQuestionShownAt,
    DateTime LastQuestionClientAnsweredAt
) : ICommand<GeneralVokiResultId>;

internal sealed class FinishVokiTakingWithSequentialAnsweringCommandHandler :
    ICommandHandler<FinishVokiTakingWithSequentialAnsweringCommand, GeneralVokiResultId>
{
    private readonly IGeneralVokisRepository _generalVokisRepository;
    private readonly IUserContext _userContext;
    private readonly IDateTimeProvider _dateTimeProvider;
    private readonly ISessionsWithSequentialAnsweringRepository _sessionsWithSequentialAnsweringRepository;
    private readonly IGeneralVokiTakenRecordsRepository _generalVokiTakenRecordsRepository;

    public FinishVokiTakingWithSequentialAnsweringCommandHandler(
        IGeneralVokisRepository generalVokisRepository,
        IUserContext userContext,
        IDateTimeProvider dateTimeProvider,
        ISessionsWithSequentialAnsweringRepository sessionsWithSequentialAnsweringRepository,
        IGeneralVokiTakenRecordsRepository generalVokiTakenRecordsRepository
    ) {
        _generalVokisRepository = generalVokisRepository;
        _userContext = userContext;
        _dateTimeProvider = dateTimeProvider;
        _sessionsWithSequentialAnsweringRepository = sessionsWithSequentialAnsweringRepository;
        _generalVokiTakenRecordsRepository = generalVokiTakenRecordsRepository;
    }

    public async Task<ErrOr<GeneralVokiResultId>> Handle(
        FinishVokiTakingWithSequentialAnsweringCommand command, CancellationToken ct
    ) {
        GeneralVoki? voki =
            await _generalVokisRepository.GetWithQuestionAnswersAndResultsAsNoTracking(command.VokiId, ct);
        if (voki is null) {
            return ErrFactory.NotFound.Voki("Cannot finish voki taking because requested Voki does not exist");
        }

        SessionWithSequentialAnswering? session =
            await _sessionsWithSequentialAnsweringRepository.GetById(command.SessionId, ct);
        if (session is null) {
            return ErrFactory.NotFound.Common("Could not finish voki taking because taking session was not started");
        }

        ErrOr<VokiTakingSessionFinishedDto> sessionFinishRes = session.FinishAndReceiveResult(
            _dateTimeProvider.UtcNow,
            sessionStartTime: command.SessionStartTime,
            clientSessionFinishedTime: command.ClientSessionFinishTime,
            _userContext.UserIdFromToken().IsSuccess(out var userId) ? new AuthenticatedUserCtx(userId) : null,
            lastQuestionId: command.LastQuestionId,
            lastQuestionOrderInVokiTaking: command.LastQuestionOrderInVokiTaking,
            lastQuestionShownAt: command.LastQuestionShownAt,
            clientLastAnsweredAt: command.LastQuestionClientAnsweredAt,
            command.LastQuestionChosenAnswers,
            (answ) => voki.GetResultIdByChosenAnswers(answ)
        );
        if (sessionFinishRes.IsErr(out var err)) {
            return err;
        }

        VokiTakingSessionFinishedDto sessionFinishedResult = sessionFinishRes.AsSuccess();
        GeneralVokiTakenRecord vokiTakenRecord = GeneralVokiTakenRecord.CreateNew(sessionFinishedResult);
        await _generalVokiTakenRecordsRepository.Add(vokiTakenRecord, ct);
        await _sessionsWithSequentialAnsweringRepository.Delete(session, ct);

        return sessionFinishedResult.ReceivedResultId;
    }
}