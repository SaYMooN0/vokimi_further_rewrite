namespace VokiRatingsService.Api.contracts;

public record class UserRatingIdsResponse(
    string[] RatingIds
) : ICreatableResponse<ImmutableHashSet<VokiRatingId>>
{
    public static ICreatableResponse<ImmutableHashSet<VokiRatingId>> Create(ImmutableHashSet<VokiRatingId> success) =>
        new UserRatingIdsResponse(
            success.Select(i => i.ToString()).ToArray()
        );
}