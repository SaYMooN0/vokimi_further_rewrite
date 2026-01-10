using VokisCatalogService.Application.common.repositories;
using VokisCatalogService.Domain.voki_aggregate;
using VokisCatalogService.Domain.voki_aggregate.voki_types;

namespace VokisCatalogService.Application.vokis.queries;

public sealed record GetVokiQuery(VokiId VokiId) : IQuery<BaseVoki>;

internal sealed class GetVokiQueryHandler : IQueryHandler<GetVokiQuery, BaseVoki>
{
    private readonly IBaseVokisRepository _baseVokisRepository;

    public GetVokiQueryHandler(IBaseVokisRepository baseVokisRepository) {
        _baseVokisRepository = baseVokisRepository;
    }

    public async Task<ErrOr<BaseVoki>> Handle(GetVokiQuery query, CancellationToken ct) {
        BaseVoki? voki = await _baseVokisRepository.GetById(query.VokiId, ct);
        if (voki is null) {
            return ErrFactory.NotFound.Voki(
                "Requested voki was not found",
                $"There is no published voki with id {query.VokiId}"
            );
        }
        return voki;
    }
}