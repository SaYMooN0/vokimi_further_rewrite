using VokiRatingsService.Domain.voki_rating_aggregate;

namespace VokiRatingsService.Api.contracts;

public record VokiRatingDataResponse(ushort Value, string UserId, DateTime DateTime)
{
    public static VokiRatingDataResponse FromRating(VokiRating rating) => new(
        rating.Current.Value,
        rating.UserId.ToString(),
        rating.Current.DateTime
    );
}