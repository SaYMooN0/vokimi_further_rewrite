using VokiRatingsService.Application.common.repositories;
using VokiRatingsService.Domain.voki_rating_aggregate;

namespace VokiRatingsService.Application.voki_ratings.queries;

public sealed record ListiRatingsForVokiQuery(VokiId VokiId) : IQuery<VokiRating[]>;

internal sealed class ListiRatingsForVokiQueryHandler : IQueryHandler<ListiRatingsForVokiQuery, VokiRating[]>
{
    private readonly IRatingsRepository _ratingsRepository;

    public ListiRatingsForVokiQueryHandler(IRatingsRepository ratingsRepository) {
        _ratingsRepository = ratingsRepository;
    }

    public async Task<ErrOr<VokiRating[]>> Handle(
        ListiRatingsForVokiQuery query, CancellationToken ct
    ) {
        return await _ratingsRepository.GetForVokiAsNoTracking(query.VokiId, ct);
    }
}