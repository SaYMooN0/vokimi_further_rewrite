using GeneralVokiTakingService.Domain.voki_taken_record_aggregate;

namespace GeneralVokiTakingService.Domain.common.interfaces.repositories;

public interface IGeneralVokiTakenRecordsRepository
{
    Task Add(GeneralVokiTakenRecord vokiTakenRecord);
}