using SharedKernel.common.vokis;
using VokisCatalogService.Application.common.repositories;

namespace VokisCatalogService.Application.vokis.queries;

public sealed record GetVokiTypeQuery(
    VokiId VokiId
) : IQuery<VokiType>;

internal sealed class GetVokiTypeQueryHandler : IQueryHandler<GetVokiTypeQuery, VokiType>
{
    private readonly IBaseVokisRepository _baseVokisRepository;

    public GetVokiTypeQueryHandler(IBaseVokisRepository baseVokisRepository) {
        _baseVokisRepository = baseVokisRepository;
    }

    public async Task<ErrOr<VokiType>> Handle(GetVokiTypeQuery query, CancellationToken ct) {
        VokiType? type = await _baseVokisRepository.GetVokiTypeById(query.VokiId, ct);
        if (type is null) {
            return ErrFactory.NotFound.Voki(
                "Requested voki was not found",
                $"There is no published voki with id {query.VokiId}"
            );
        }

        return type;
    }
}