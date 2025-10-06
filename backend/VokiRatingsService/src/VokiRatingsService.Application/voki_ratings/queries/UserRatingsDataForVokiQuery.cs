using SharedKernel.auth;
using VokiRatingsService.Application.common.repositories;
using VokiRatingsService.Domain.app_user_aggregate;
using VokiRatingsService.Domain.voki_rating_aggregate;

namespace VokiRatingsService.Application.voki_ratings.queries;

public sealed record UserRatingsDataForVokiQuery(VokiId VokiId) : IQuery<UserRatingsDataForVokiQueryResult>;

internal sealed class UserRatingsDataForVokiQueryHandler :
    IQueryHandler<UserRatingsDataForVokiQuery, UserRatingsDataForVokiQueryResult>
{
    private readonly IUserContext _userContext;
    private readonly IRatingsRepository _ratingsRepository;
    private readonly IAppUsersRepository _appUsersRepository;

    public UserRatingsDataForVokiQueryHandler(
        IUserContext userContext,
        IRatingsRepository ratingsRepository,
        IAppUsersRepository appUsersRepository
    ) {
        _userContext = userContext;
        _ratingsRepository = ratingsRepository;
        _appUsersRepository = appUsersRepository;
    }

    public Task<ErrOr<UserRatingsDataForVokiQueryResult>> Handle(
        UserRatingsDataForVokiQuery query, CancellationToken ct
    ) =>
        _userContext.UserIdFromToken().Match(
            successFunc: userId => GetRatingsDataForAuthenticatedUser(userId, query.VokiId, ct),
            errorFunc: _ => GetRatingsDataForNotAuthenticatedUser(query.VokiId, ct)
        );

    private async Task<ErrOr<UserRatingsDataForVokiQueryResult>> GetRatingsDataForAuthenticatedUser(
        AppUserId userId, VokiId vokiId, CancellationToken ct
    ) {
        VokiRating[] ratings = (await _ratingsRepository.GetForVokiAsNoTracking(vokiId, ct));
        VokiRating? userRating = ratings.SingleOrDefault(r => r.UserId == userId);
        bool hasTaken = false;

        if (userRating is not null) {
            hasTaken = true;
        }
        else {
            AppUser user = (await _appUsersRepository.GetByIdAsNoTracking(userId, ct))!;
            hasTaken = user.HasTaken(vokiId);
        }


        return new UserRatingsDataForVokiQueryResult(hasTaken, ratings);
    }

    private async Task<ErrOr<UserRatingsDataForVokiQueryResult>> GetRatingsDataForNotAuthenticatedUser(
        VokiId vokiId, CancellationToken ct
    ) => new UserRatingsDataForVokiQueryResult(
        false,
        await _ratingsRepository.GetForVokiAsNoTracking(vokiId, ct)
    );
}

public sealed record UserRatingsDataForVokiQueryResult(bool UserHasTaken, VokiRating[] Ratings);