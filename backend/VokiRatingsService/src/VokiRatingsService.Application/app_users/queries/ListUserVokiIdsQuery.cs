using SharedKernel.auth;
using VokiRatingsService.Application.common.repositories;
using VokiRatingsService.Domain.app_user_aggregate;

namespace VokiRatingsService.Application.app_users.queries;

public sealed record ListUserVokiIdsQuery() : IQuery<ImmutableHashSet<VokiRatingId>>;

internal sealed class VokiAverageRatingQueryHandler :
    IQueryHandler<ListUserVokiIdsQuery, ImmutableHashSet<VokiRatingId>>
{
    private readonly IAppUsersRepository _appUsersRepository;
    private readonly IUserContext _userContext;

    public VokiAverageRatingQueryHandler(IAppUsersRepository appUsersRepository, IUserContext userContext) {
        _appUsersRepository = appUsersRepository;
        _userContext = userContext;
    }

    public async Task<ErrOr<ImmutableHashSet<VokiRatingId>>> Handle(ListUserVokiIdsQuery query, CancellationToken ct) {
        var userIdOrErr = _userContext.UserIdFromToken();
        if (userIdOrErr.IsErr(out var err)) {
            return ErrFactory.AuthRequired("To see your rated Vokis you need to log into your account");
        }

        AppUserId userId = userIdOrErr.AsSuccess();
        AppUser? user = await _appUsersRepository.GetByIdAsNoTracking(userId, ct);
        if (user is null) {
            return ErrFactory.NotFound.User("User account not found");
        }

        return user.RatingIds;
    }
}