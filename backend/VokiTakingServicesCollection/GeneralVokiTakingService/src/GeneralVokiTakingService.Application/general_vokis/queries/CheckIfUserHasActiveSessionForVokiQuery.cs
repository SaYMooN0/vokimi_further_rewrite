using GeneralVokiTakingService.Application.dtos;

namespace GeneralVokiTakingService.Application.general_vokis.queries;

public record CheckIfUserHasActiveSessionForVokiQuery(
    VokiId VokiId
) : IQuery<CheckIfUserHasActiveSessionForVokiQueryResult>;

internal sealed class CheckIfUserHasActiveSessionForVokiQueryHandler :
    IQueryHandler<CheckIfUserHasActiveSessionForVokiQuery, CheckIfUserHasActiveSessionForVokiQueryResult>
{
    public Task<ErrOr<CheckIfUserHasActiveSessionForVokiQueryResult>> Handle(
        CheckIfUserHasActiveSessionForVokiQuery query, CancellationToken ct
    ) {
        return Task.FromResult(
            ErrOr<CheckIfUserHasActiveSessionForVokiQueryResult>.Success(
                CheckIfUserHasActiveSessionForVokiQueryResult.CreateNoSession()
            )
        );
    }
}

public sealed class CheckIfUserHasActiveSessionForVokiQueryResult
{
    private CheckIfUserHasActiveSessionForVokiQueryResult(SavedActiveVokiTakingSessionDto? activeSessionDto) {
        ActiveSessionDto = activeSessionDto;
    }

    private SavedActiveVokiTakingSessionDto? ActiveSessionDto { get; }

    public T Match<T>(Func<SavedActiveVokiTakingSessionDto, T> sessionExistsFunc, Func<T> noSessionFunc) =>
        ActiveSessionDto is null ? noSessionFunc() : sessionExistsFunc(ActiveSessionDto);

    public static CheckIfUserHasActiveSessionForVokiQueryResult CreateNoSession() => new(null);

    public static CheckIfUserHasActiveSessionForVokiQueryResult CreateWithSessionExists(SavedActiveVokiTakingSessionDto session)
        => new(session);
}