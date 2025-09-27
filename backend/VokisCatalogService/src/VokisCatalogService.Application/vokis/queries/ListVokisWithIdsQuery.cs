using VokisCatalogService.Application.common.repositories;
using VokisCatalogService.Domain.voki_aggregate;

namespace VokisCatalogService.Application.vokis.queries;
public sealed record ListVokisWithIdsQuery(VokiId[] VokiIds) :
    IQuery<BaseVoki[]>;

internal sealed class ListVokisWithIdsQueryHandler : IQueryHandler<ListVokisWithIdsQuery, BaseVoki[]>
{
    private readonly IBaseVokisRepository _baseVokisRepository;
    public ListVokisWithIdsQueryHandler(IBaseVokisRepository baseVokisRepository) {
        _baseVokisRepository = baseVokisRepository;
    }
    public async Task<ErrOr<BaseVoki[]>> Handle(ListVokisWithIdsQuery withIdsQuery, CancellationToken ct) {
        return await _baseVokisRepository.GetMultipleByIdAsNoTracking(withIdsQuery.VokiIds);
    }
}