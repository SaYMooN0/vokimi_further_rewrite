using CoreVokiCreationService.Domain.common.interfaces.repositories;

namespace CoreVokiCreationService.Infrastructure.persistence.repositories;

internal class DraftVokiRepository : IDraftVokiRepository
{
    private readonly CoreVokiCreationDbContext _db;

    public DraftVokiRepository(CoreVokiCreationDbContext db) {
        _db = db;
    }
}