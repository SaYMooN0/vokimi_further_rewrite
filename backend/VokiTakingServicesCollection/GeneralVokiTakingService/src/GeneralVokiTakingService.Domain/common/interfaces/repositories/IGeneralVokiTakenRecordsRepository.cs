using GeneralVokiTakingService.Domain.voki_taken_record_aggregate;

namespace GeneralVokiTakingService.Domain.common.interfaces.repositories;

public interface IGeneralVokiTakenRecordsRepository
{
    Task Add(GeneralVokiTakenRecord vokiTakenRecord);
    Task<GeneralVokiTakenRecord[]> ForVokiByUserAsNoTracking(VokiId vokiId, AppUserId userId, CancellationToken ct);
    Task<Dictionary<GeneralVokiResultId, uint>> GetResultIdsToCountForVoki(VokiId vokiId, CancellationToken ct);
}