using ApplicationShared;
using ApplicationShared.messaging.pipeline_behaviors;
using VokiRatingsService.Application.common.repositories;

namespace VokiRatingsService.Application.app_users.queries;

public sealed record ListUserRatedVokisQuery() :
    IQuery<VokiIdWithLastRatingDto[]>,
    IWithAuthCheckStep
{
    public Err UnauthenticatedErr => ErrFactory.AuthRequired(
        "To see your rated Vokis you need to log into your account"
    );
}

internal sealed class ListUserRatedVokisQueryHandler :
    IQueryHandler<ListUserRatedVokisQuery, VokiIdWithLastRatingDto[]>
{
    private readonly IUserContext _userContext;
    private readonly IRatingsRepository _ratingsRepository;

    public ListUserRatedVokisQueryHandler(IUserContext userContext, IRatingsRepository ratingsRepository) {
        _userContext = userContext;
        _ratingsRepository = ratingsRepository;
    }


    public async Task<ErrOr<VokiIdWithLastRatingDto[]>> Handle(ListUserRatedVokisQuery query, CancellationToken ct) {
        return await _ratingsRepository.ListIdsOfVokiRatedByUser(
            new AuthenticatedUserContext(_userContext.AuthenticatedUserId), ct);
    }
}