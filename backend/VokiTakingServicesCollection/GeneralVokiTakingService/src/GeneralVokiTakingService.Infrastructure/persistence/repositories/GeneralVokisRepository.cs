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

    public async Task Add(GeneralVoki voki) {
        await _db.Vokis.AddAsync(voki);
        await _db.SaveChangesAsync();
    }


    public Task<GeneralVoki?> GetWithQuestionAnswersAsNoTracking(VokiId vokiId) => _db.Vokis
        .AsNoTracking()
        .Include(v => v.Questions)
        .ThenInclude(q => q.Answers)
        .FirstOrDefaultAsync(v => v.Id == vokiId);

    public Task<GeneralVoki?> GetWithQuestionAnswersAndResultsAsNoTracking(VokiId vokiId) => _db.Vokis
        .AsNoTracking()
        .Include(v => v.Questions)
        .ThenInclude(q => q.Answers)
        .AsSplitQuery()
        .Include(v => EF.Property<IReadOnlyCollection<VokiResult>>(v, "Results"))
        .FirstOrDefaultAsync(v => v.Id == vokiId);
}