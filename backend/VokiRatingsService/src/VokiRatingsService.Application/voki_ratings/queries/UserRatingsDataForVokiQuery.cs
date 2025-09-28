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
    private readonly IVokiRatingsRepository _vokiRatingsRepository;
    private readonly IAppUsersRepository _appUsersRepository;

    public UserRatingsDataForVokiQueryHandler(
        IUserContext userContext,
        IVokiRatingsRepository vokiRatingsRepository,
        IAppUsersRepository appUsersRepository
    ) {
        _userContext = userContext;
        _vokiRatingsRepository = vokiRatingsRepository;
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
        VokiRating[] ratings = await _vokiRatingsRepository.GetForVokiAsNoTracking(vokiId, ct);
        VokiRating? userRating = ratings.SingleOrDefault(r => r.UserId == userId);

        if (userRating is not null) {
            var others = ratings
                .Where(r => r.UserId != userId)
                .ToImmutableArray();

            return new UserRatingsDataForVokiQueryResult(
                UserHasTaken: true,
                UserRating: userRating,
                OtherUserRatings: others
            );
        }

        AppUser user = (await _appUsersRepository.GetByIdAsNoTracking(userId, ct))!;
        bool hasTaken = user.HasTaken(vokiId);

        return new UserRatingsDataForVokiQueryResult(
            UserHasTaken: hasTaken,
            UserRating: null,
            OtherUserRatings: ratings.ToImmutableArray()
        );
    }

    private async Task<ErrOr<UserRatingsDataForVokiQueryResult>> GetRatingsDataForNotAuthenticatedUser(
        VokiId vokiId, CancellationToken ct
    ) {
        var ratings = await _vokiRatingsRepository.GetForVokiAsNoTracking(vokiId, ct);
        return UserRatingsDataForVokiQueryResult.ForNotAuthenticatedUser(ratings.ToImmutableArray());
    }
}

public sealed record UserRatingsDataForVokiQueryResult(
    bool UserHasTaken,
    VokiRating? UserRating,
    ImmutableArray<VokiRating> OtherUserRatings
)
{
    public static UserRatingsDataForVokiQueryResult ForNotAuthenticatedUser(
        ImmutableArray<VokiRating> otherUserRatings
    ) => new(false, null, otherUserRatings);

    public double AverageRating() {
        double sum = OtherUserRatings.Sum(r => r.Current.Value);
        double count = OtherUserRatings.Length;
        if (UserRating is not null) {
            sum += UserRating.Current.Value;
            count += 1;
        }

        return count == 0 ? 0 : sum / count;
    }
}