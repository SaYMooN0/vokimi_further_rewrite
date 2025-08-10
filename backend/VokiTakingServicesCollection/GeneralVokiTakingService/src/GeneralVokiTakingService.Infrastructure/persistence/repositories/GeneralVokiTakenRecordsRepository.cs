using GeneralVokiTakingService.Domain.common.interfaces.repositories;

namespace GeneralVokiTakingService.Infrastructure.persistence.repositories;

internal class GeneralVokiTakenRecordsRepository : IGeneralVokiTakenRecordsRepository
{
    private readonly GeneralVokiTakingDbContext _db;

    public GeneralVokiTakenRecordsRepository(GeneralVokiTakingDbContext db) {
        _db = db;
    }

}