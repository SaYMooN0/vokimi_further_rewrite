using GeneralVokiTakingService.Domain.general_voki_aggregate;

namespace GeneralVokiTakingService.Domain.common.interfaces.repositories;

public interface IGeneralVokisRepository
{
    Task<GeneralVoki?> GetByIdAsNoTracking(VokiId vokiId);

    Task Add(GeneralVoki voki);
}