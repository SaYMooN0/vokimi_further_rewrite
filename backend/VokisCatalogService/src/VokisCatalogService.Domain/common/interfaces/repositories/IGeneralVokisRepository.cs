using VokisCatalogService.Domain.voki_aggregate.voki_types;

namespace VokisCatalogService.Domain.common.interfaces.repositories;

public interface IGeneralVokisRepository
{
    Task Add(GeneralVoki voki);
}