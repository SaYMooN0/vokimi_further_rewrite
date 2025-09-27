using VokisCatalogService.Domain.voki_aggregate.voki_types;

namespace VokisCatalogService.Application.common.repositories;

public interface IGeneralVokisRepository
{
    Task Add(GeneralVoki voki);
}