using VokisCatalogService.Domain.common.interfaces.repositories;
using VokisCatalogService.Domain.voki_aggregate;

namespace VokisCatalogService.Application.vokis.queries;


public sealed record ListAllVokisQuery() : IQuery<BaseVoki[]>;

internal sealed class ListAllVokisQueryHandler : IQueryHandler<ListAllVokisQuery, BaseVoki[]>
{
    private readonly IBaseVokisRepository _baseVokisRepository;

    public ListAllVokisQueryHandler(IBaseVokisRepository baseVokisRepository) {
        _baseVokisRepository = baseVokisRepository;
    }

    public async Task<ErrOr<BaseVoki[]>> Handle(ListAllVokisQuery query, CancellationToken ct) =>
        await _baseVokisRepository.GetAllSortedAsNoTracking();
     
}