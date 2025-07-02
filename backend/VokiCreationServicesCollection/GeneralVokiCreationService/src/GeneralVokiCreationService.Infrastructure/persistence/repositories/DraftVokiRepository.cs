using CoreVokiCreationService.Domain.common.interfaces.repositories;

namespace GeneralVokiCreationService.Infrastructure.persistence.repositories;

internal class DraftVokiRepository : IDraftVokiRepository
{
    private readonly GeneralVokiCreationDbContext _db;

    public DraftVokiRepository(GeneralVokiCreationDbContext db) {
        _db = db;
    }
}