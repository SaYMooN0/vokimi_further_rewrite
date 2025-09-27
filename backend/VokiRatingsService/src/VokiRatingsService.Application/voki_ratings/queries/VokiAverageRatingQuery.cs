using VokiRatingsService.Application.common.repositories;

namespace VokiRatingsService.Application.voki_ratings.queries;

public sealed record VokiAverageRatingQuery(VokiId VokiId, GeneralVokiResultId ResultId) : IQuery<double>;

internal sealed class VokiAverageRatingQueryHandler : IQueryHandler<VokiAverageRatingQuery, double>
{
    private readonly IVokiRatingsRepository _vokiRatingsRepository;

    public VokiAverageRatingQueryHandler(IVokiRatingsRepository vokiRatingsRepository) {
        _vokiRatingsRepository = vokiRatingsRepository;
    }

    public async Task<ErrOr<double>> Handle(VokiAverageRatingQuery query, CancellationToken ct) {
        (uint ratingsSum, uint ratingsCount) = await _vokiRatingsRepository.GetRatingsSumAndCountForVoki(query.VokiId);
        if (ratingsCount == 0) {
            return 0;
        }
        return ratingsSum / (double)ratingsCount;
    }
}