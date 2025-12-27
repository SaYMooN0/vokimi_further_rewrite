using ApplicationShared;
using ApplicationShared.messaging.pipeline_behaviors;
using VokiRatingsService.Application.common.repositories;
using VokiRatingsService.Domain.voki_rating_aggregate;

namespace VokiRatingsService.Application.voki_ratings.queries;

public sealed record ManageVokiRatingsDataQuery(
    VokiId VokiId
) : IQuery<ManageVokiRatingsDataQueryResult>,
    IWithAuthCheckStep;

internal sealed class ManageVokiRatingsDataQueryHandler :
    IQueryHandler<ManageVokiRatingsDataQuery, ManageVokiRatingsDataQueryResult>
{
    private readonly IUserContext _userContext;
    private readonly IRatingsRepository _ratingsRepository;
    private readonly IAppUsersRepository _appUsersRepository;
    private readonly IVokisRepository _vokisRepository;


    public Task<ErrOr<ManageVokiRatingsDataQueryResult>> Handle(
        ManageVokiRatingsDataQuery query, CancellationToken ct
    ) { }
}

public sealed record ManageVokiRatingsDataQueryResult(
    double AverageRating,
    uint RatingsCount,
    Dictionary<ushort, uint> ValueToCountDistribution
);