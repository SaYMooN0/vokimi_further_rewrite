namespace VokiRatingsService.Application.voki_ratings.queries;

public sealed record UserRatingsDataForVokiQuery(VokiId VokiId, GeneralVokiResultId ResultId) : IQuery<UserRatingsDataForVokiQueryResult>;

internal sealed class UserRatingsDataForVokiQueryHandler : IQueryHandler<UserRatingsDataForVokiQuery, UserRatingsDataForVokiQueryResult>
{
    public Task<ErrOr<UserRatingsDataForVokiQueryResult>> Handle(UserRatingsDataForVokiQuery query, CancellationToken ct) {
        return Task.FromResult<ErrOr<UserRatingsDataForVokiQueryResult>>(new UserRatingsDataForVokiQueryResult());
    }

}
public sealed record UserRatingsDataForVokiQueryResult();