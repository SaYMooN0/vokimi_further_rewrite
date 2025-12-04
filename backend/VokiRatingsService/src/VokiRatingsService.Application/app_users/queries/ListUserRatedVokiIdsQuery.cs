using ApplicationShared;
using ApplicationShared.messaging.pipeline_behaviors;
using VokiRatingsService.Application.common.repositories;

namespace VokiRatingsService.Application.app_users.queries;

public sealed record ListUserRatedVokiIdsQuery() :
    IQuery<VokiIdWithLastRatingDto[]>,
    IWithAuthCheckStep
{
    public Err UnauthenticatedErr => ErrFactory.AuthRequired(
        "To see your rated Vokis you need to log into your account"
    );
}

internal sealed class ListUserRatedVokiIdsQueryHandler :
    IQueryHandler<ListUserRatedVokiIdsQuery, VokiIdWithLastRatingDto[]>
{
    private readonly IUserContext _userContext;
    private readonly IRatingsRepository _ratingsRepository;

    public ListUserRatedVokiIdsQueryHandler(IUserContext userContext, IRatingsRepository ratingsRepository) {
        _userContext = userContext;
        _ratingsRepository = ratingsRepository;
    }


    public async Task<ErrOr<VokiIdWithLastRatingDto[]>> Handle(ListUserRatedVokiIdsQuery query, CancellationToken ct) {
        return await _ratingsRepository.OrderedIdsOfVokiRatedByUser(
            new AuthenticatedUserContext(_userContext.AuthenticatedUserId), ct);
    }
}