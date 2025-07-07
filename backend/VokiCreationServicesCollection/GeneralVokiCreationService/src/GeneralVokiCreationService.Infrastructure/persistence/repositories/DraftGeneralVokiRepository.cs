using GeneralVokiCreationService.Domain.draft_general_voki_aggregate;
using GeneralVokiCreationService.Domain.repositories;

namespace GeneralVokiCreationService.Infrastructure.persistence.repositories;

internal class DraftGeneralVokiRepository : IDraftGeneralVokiRepository
{
    private readonly GeneralVokiCreationDbContext _db;

    public DraftGeneralVokiRepository(GeneralVokiCreationDbContext db) {
        _db = db;
    }

    public Task Add(DraftGeneralVoki generalVoki) {
        _db.Vokis.Add(generalVoki);
        return _db.SaveChangesAsync();
    }
}