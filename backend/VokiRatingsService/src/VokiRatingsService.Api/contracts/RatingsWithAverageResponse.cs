using VokiRatingsService.Domain.voki_rating_aggregate;

namespace VokiRatingsService.Api.contracts;

public record class RatingsWithAverageResponse(
    VokiRatingDataResponse[] Ratings,
    double AverageRating
) : ICreatableResponse<VokiRating[]>
{
    public static ICreatableResponse<VokiRating[]> Create(VokiRating[] ratings) => new RatingsWithAverageResponse(
        ratings.Select(VokiRatingDataResponse.FromRating).ToArray(),
        CalculateAverageRating(ratings)
    );

    private static double CalculateAverageRating(VokiRating[] ratings) {
        if (ratings.Length == 0) {
            return 0;
        }

        double sum = ratings.Sum(r => r.Current.Value);
        return sum / ratings.Length;
    }
}