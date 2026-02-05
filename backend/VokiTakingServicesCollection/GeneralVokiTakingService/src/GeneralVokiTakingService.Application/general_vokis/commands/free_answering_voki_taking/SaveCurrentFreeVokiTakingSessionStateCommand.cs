using ApplicationShared;
using GeneralVokiTakingService.Application.common.repositories.taking_sessions;
using GeneralVokiTakingService.Domain.common;
using GeneralVokiTakingService.Domain.voki_taking_session_aggregate;

namespace GeneralVokiTakingService.Application.general_vokis.commands.free_answering_voki_taking;

public record SaveCurrentFreeVokiTakingSessionStateCommand(
    VokiId VokiId,
    VokiTakingSessionId SessionId,
    ImmutableDictionary<GeneralVokiQuestionId, ImmutableHashSet<GeneralVokiAnswerId>> ChosenAnswers
) : ICommand;

internal sealed class SaveCurrentFreeVokiTakingSessionStateCommandHandler
    : ICommandHandler<SaveCurrentFreeVokiTakingSessionStateCommand>
{
    private readonly ISessionsWithFreeAnsweringRepository _sessionsWithFreeAnsweringRepository;
    private readonly IUserCtxProvider _userCtxProvider;

    public SaveCurrentFreeVokiTakingSessionStateCommandHandler(
        ISessionsWithFreeAnsweringRepository sessionsWithFreeAnsweringRepository,
        IUserCtxProvider userCtxProvider
    ) {
        _sessionsWithFreeAnsweringRepository = sessionsWithFreeAnsweringRepository;
        _userCtxProvider = userCtxProvider;
    }

    public async Task<ErrOrNothing> Handle(SaveCurrentFreeVokiTakingSessionStateCommand command, CancellationToken ct) {
        SessionWithFreeAnswering? session = await _sessionsWithFreeAnsweringRepository.GetByIdForUpdate(command.SessionId, ct);

        if (session is null) {
            return ErrFactory.NotFound.Common("Session not found");
        }


        ErrOrNothing saveRes = session.SaveAnswers(_userCtxProvider.Current, command.VokiId, command.ChosenAnswers);
        if (saveRes.IsErr(out var err)) {
            return err;
        }

        await _sessionsWithFreeAnsweringRepository.Update(session, ct);
        return ErrOrNothing.Nothing;
    }
}