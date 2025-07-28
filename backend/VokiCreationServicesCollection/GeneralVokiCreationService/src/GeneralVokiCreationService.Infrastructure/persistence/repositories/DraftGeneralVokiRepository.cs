using GeneralVokiCreationService.Domain.common.interfaces.repositories;
using GeneralVokiCreationService.Domain.draft_general_voki_aggregate;
using Microsoft.EntityFrameworkCore;

namespace GeneralVokiCreationService.Infrastructure.persistence.repositories;

internal class DraftGeneralVokiRepository : IDraftGeneralVokiRepository
{
    private readonly GeneralVokiCreationDbContext _db;

    public DraftGeneralVokiRepository(GeneralVokiCreationDbContext db) {
        _db = db;
    }

    public Task Add(DraftGeneralVoki voki) {
        _db.Vokis.Add(voki);
        return _db.SaveChangesAsync();
    }

    public Task Update(DraftGeneralVoki voki) {
        _db.Vokis.Update(voki);
        return _db.SaveChangesAsync();
    }

    public Task<DraftGeneralVoki?> GetByIdAsNoTracking(VokiId vokiId) => _db.Vokis
        .AsNoTracking()
        .FirstOrDefaultAsync(v => v.Id == vokiId);

    public Task<DraftGeneralVoki?> GetWithQuestionsAsNoTracking(VokiId vokiId) => _db.Vokis
        .AsNoTracking()
        .Include(v => EF.Property<List<VokiQuestion>>(v, "_questions"))
        .FirstOrDefaultAsync(v => v.Id == vokiId);

    public Task<DraftGeneralVoki?> GetWithQuestions(VokiId vokiId) => _db.Vokis
        .Include(v => EF.Property<List<VokiQuestion>>(v, "_questions"))
        .FirstOrDefaultAsync(v => v.Id == vokiId);

    public Task<DraftGeneralVoki?> GetWithQuestionAnswersAsNoTracking(VokiId vokiId) => _db.Vokis
        .AsNoTracking()
        .Include(v => EF.Property<List<VokiQuestion>>(v, "_questions"))
        .ThenInclude(q => EF.Property<List<VokiQuestionAnswer>>(q, "_answers"))
        .FirstOrDefaultAsync(v => v.Id == vokiId);

    public Task<DraftGeneralVoki?> GetWithQuestionAnswers(VokiId vokiId) => _db.Vokis
        .Include(v => EF.Property<List<VokiQuestion>>(v, "_questions"))
        .ThenInclude(q => EF.Property<List<VokiQuestionAnswer>>(q, "_answers"))
        .FirstOrDefaultAsync(v => v.Id == vokiId);


    public Task<DraftGeneralVoki?> GetById(VokiId vokiId) => _db.Vokis
        .FirstOrDefaultAsync(v => v.Id == vokiId);
}