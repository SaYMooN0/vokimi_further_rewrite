using GeneralVokiTakingService.Domain.common.interfaces.repositories;
using GeneralVokiTakingService.Domain.voki_taken_record_aggregate;

namespace GeneralVokiTakingService.Infrastructure.persistence.repositories;

internal class GeneralVokiTakenRecordsRepository : IGeneralVokiTakenRecordsRepository
{
    private readonly GeneralVokiTakingDbContext _db;

    public GeneralVokiTakenRecordsRepository(GeneralVokiTakingDbContext db) {
        _db = db;
    }

    public async Task Add(GeneralVokiTakenRecord vokiTakenRecord) {
        await _db.VokiTakenRecords.AddAsync(vokiTakenRecord);
        await _db.SaveChangesAsync();
    }
}