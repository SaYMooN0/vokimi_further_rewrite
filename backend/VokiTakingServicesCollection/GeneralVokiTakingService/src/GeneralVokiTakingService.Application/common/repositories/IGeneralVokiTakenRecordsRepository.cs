using GeneralVokiTakingService.Domain.voki_taken_record_aggregate;

namespace GeneralVokiTakingService.Application.common.repositories;

public interface IGeneralVokiTakenRecordsRepository
{
    Task Add(GeneralVokiTakenRecord vokiTakenRecord, CancellationToken ct);
    Task<GeneralVokiTakenRecord[]> ForVokiByUserAsNoTracking(VokiId vokiId, AppUserId userId, CancellationToken ct);
    Task<Dictionary<GeneralVokiResultId, uint>> GetResultIdsToCountForVoki(VokiId vokiId, CancellationToken ct);
}