using VokiRatingsService.Application.common.repositories;
using VokiRatingsService.Domain.voki_rating_aggregate;

namespace VokiRatingsService.Application.voki_ratings.queries;

public sealed record ListVokiRatingsForVokiQuery(VokiId VokiId) : IQuery<VokiRating[]>;

internal sealed class ListVokiRatingsForVokiQueryHandler : IQueryHandler<ListVokiRatingsForVokiQuery, VokiRating[]>
{
    private readonly IRatingsRepository _ratingsRepository;

    public ListVokiRatingsForVokiQueryHandler(IRatingsRepository ratingsRepository) {
        _ratingsRepository = ratingsRepository;
    }

    public async Task<ErrOr<VokiRating[]>> Handle(
        ListVokiRatingsForVokiQuery query, CancellationToken ct
    ) {
        return await _ratingsRepository.GetForVokiAsNoTracking(query.VokiId, ct);
    }
}