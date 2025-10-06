using SharedKernel.auth;
using VokiRatingsService.Application.common.repositories;

namespace VokiRatingsService.Application.app_users.queries;

public sealed record ListUserRatedVokiIdsQuery() : IQuery<VokiIdWithRatingDateDto[]>;

internal sealed class ListUserRatedVokiIdsQueryHandler :
    IQueryHandler<ListUserRatedVokiIdsQuery, VokiIdWithRatingDateDto[]>
{
    private readonly IUserContext _userContext;
    private readonly IRatingsRepository _ratingsRepository;

    public ListUserRatedVokiIdsQueryHandler(IUserContext userContext, IRatingsRepository ratingsRepository) {
        _userContext = userContext;
        _ratingsRepository = ratingsRepository;
    }


    public async Task<ErrOr<VokiIdWithRatingDateDto[]>> Handle(ListUserRatedVokiIdsQuery query, CancellationToken ct) {
        var userIdOrErr = _userContext.UserIdFromToken();
        if (userIdOrErr.IsErr(out var err)) {
            return ErrFactory.AuthRequired("To see your rated Vokis you need to log into your account");
        }

        AppUserId userId = userIdOrErr.AsSuccess();
        return await _ratingsRepository.OrderedIdsOfVokiRatedByUser(userId, ct);
    }
}