using VokiRatingsService.Application.common.repositories;

namespace VokiRatingsService.Application.voki_ratings.queries;

public sealed record VokiOtherUsersRatingsWithAverageQuery(VokiId VokiId)
    : IQuery<VokiOtherUsersRatingsWithAverageQueryResult>;

internal sealed class VokiAverageRatingQueryHandler :
    IQueryHandler<VokiOtherUsersRatingsWithAverageQuery, VokiOtherUsersRatingsWithAverageQueryResult>
{
    private readonly IVokiRatingsRepository _vokiRatingsRepository;

    public VokiAverageRatingQueryHandler(IVokiRatingsRepository vokiRatingsRepository) {
        _vokiRatingsRepository = vokiRatingsRepository;
    }

    public async Task<ErrOr<VokiOtherUsersRatingsWithAverageQueryResult>> Handle(
        VokiOtherUsersRatingsWithAverageQuery query, CancellationToken ct
    ) {
        (uint ratingsSum, uint ratingsCount) = await _vokiRatingsRepository
            .GetRatingsSumAndCountForVoki(query.VokiId, ct);

        if (ratingsCount == 0) {
            return new VokiOtherUsersRatingsWithAverageQueryResult(0);
        }

        return new VokiOtherUsersRatingsWithAverageQueryResult(ratingsSum / (double)ratingsCount);
    }
}

public record VokiOtherUsersRatingsWithAverageQueryResult(
    double AverageRating
);