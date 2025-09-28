namespace VokiRatingsService.Api.contracts;

public record class RatingsWithAverageResponse(
    VokiRatingResponse[] Ratings,
    double AverageRating
);

