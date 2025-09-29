using VokiRatingsService.Application.common.repositories;
using VokiRatingsService.Domain.voki_rating_aggregate;

namespace VokiRatingsService.Application.voki_ratings.queries;

public sealed record ListVokiRatingsForVokiQuery(VokiId VokiId) : IQuery<VokiRating[]>;

internal sealed class ListVokiRatingsForVokiQueryHandler : IQueryHandler<ListVokiRatingsForVokiQuery, VokiRating[]>
{
    private readonly IVokiRatingsRepository _vokiRatingsRepository;

    public ListVokiRatingsForVokiQueryHandler(IVokiRatingsRepository vokiRatingsRepository) {
        _vokiRatingsRepository = vokiRatingsRepository;
    }

    public async Task<ErrOr<VokiRating[]>> Handle(
        ListVokiRatingsForVokiQuery query, CancellationToken ct
    ) {
        return await _vokiRatingsRepository.GetForVokiAsNoTracking(query.VokiId, ct);
    }
}