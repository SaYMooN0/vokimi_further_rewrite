using GeneralVokiCreationService.Domain.draft_voki_aggregate;
using GeneralVokiCreationService.Domain.repositories;

namespace GeneralVokiCreationService.Infrastructure.persistence.repositories;

internal class DraftVokiRepository : IDraftVokiRepository
{
    private readonly GeneralVokiCreationDbContext _db;

    public DraftVokiRepository(GeneralVokiCreationDbContext db) {
        _db = db;
    }

    public Task Add(DraftVoki voki) {
        _db.Vokis.Add(voki);
        return _db.SaveChangesAsync();
    }
}