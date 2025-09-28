using VokiRatingsService.Domain.voki_rating_aggregate;

namespace VokiRatingsService.Api.contracts;

public record VokiRatingResponse(ushort Value, string UserId, DateTime DateTime)
{
    public static VokiRatingResponse FromRating(VokiRating rating) => new(
        rating.Current.Value,
        rating.UserId.ToString(),
        rating.Current.DateTime
    );
}