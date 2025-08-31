using GeneralVokiTakingService.Domain.common.interfaces.repositories;
using GeneralVokiTakingService.Domain.general_voki_aggregate;
using Microsoft.EntityFrameworkCore;

namespace GeneralVokiTakingService.Infrastructure.persistence.repositories;

internal class GeneralVokisRepository : IGeneralVokisRepository
{
    private readonly GeneralVokiTakingDbContext _db;

    public GeneralVokisRepository(GeneralVokiTakingDbContext db) {
        _db = db;
    }

    public Task Add(GeneralVoki voki) {
        _db.Vokis.Add(voki);
        return _db.SaveChangesAsync();
    }


    public Task<GeneralVoki?> GetByIdAsNoTracking(VokiId vokiId) => _db.Vokis
        .AsNoTracking()
        .FirstOrDefaultAsync(v => v.Id == vokiId);

    public Task<GeneralVoki?> GetWithQuestionAnswersAsNoTracking(VokiId vokiId) => _db.Vokis
        .AsNoTracking()
        .Include(v => v.Questions)
        .ThenInclude(q => q.Answers)
        .FirstOrDefaultAsync(v => v.Id == vokiId);
}