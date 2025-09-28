using ApiShared;
using VokiRatingsService.Application.voki_ratings.queries;

namespace VokiRatingsService.Api.contracts;

public record class UserRatingsDataForVokiResponse(
    bool UserHasTaken,
    VokiRatingResponse? UserRating,
    RatingsWithAverageResponse RatingsWithAverage
) : ICreatableResponse<UserRatingsDataForVokiQueryResult>
{
    public static ICreatableResponse<UserRatingsDataForVokiQueryResult> Create(
        UserRatingsDataForVokiQueryResult success
    ) => new UserRatingsDataForVokiResponse(
        success.UserHasTaken,
        success.UserRating is null ? null : VokiRatingResponse.FromRating(success.UserRating),
        new RatingsWithAverageResponse(
            success.OtherUserRatings.Select(VokiRatingResponse.FromRating).ToArray(),
            success.AverageRating()
        )
    );
}