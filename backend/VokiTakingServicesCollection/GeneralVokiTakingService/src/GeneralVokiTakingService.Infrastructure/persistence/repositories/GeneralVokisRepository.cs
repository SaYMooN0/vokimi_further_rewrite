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

    public async Task Add(GeneralVoki voki, CancellationToken ct) {
        await _db.Vokis.AddAsync(voki, ct);
        await _db.SaveChangesAsync(ct);
    }


    public Task<GeneralVoki?> GetWithQuestionAnswersAsNoTracking(VokiId vokiId, CancellationToken ct) =>
        _db.Vokis
            .AsNoTracking()
            .Include(v => v.Questions)
            .ThenInclude(q => q.Answers)
            .FirstOrDefaultAsync(v => v.Id == vokiId, cancellationToken: ct);

    public Task<GeneralVoki?> GetWithResultsByIdAsNoTracking(VokiId vokiId, CancellationToken ct) =>
        _db.Vokis
            .AsNoTracking()
            .Include(v => EF.Property<List<VokiResult>>(v, "_results"))
            .FirstOrDefaultAsync(v => v.Id == vokiId, cancellationToken: ct);

    public Task<GeneralVoki?> GetWithQuestionAnswersAndResultsAsNoTracking(VokiId vokiId, CancellationToken ct) =>
        _db.Vokis
            .AsNoTracking()
            .Include(v => v.Questions)
            .ThenInclude(q => q.Answers)
            .AsSplitQuery()
            .Include(v => EF.Property<IReadOnlyCollection<VokiResult>>(v, "_results"))
            .FirstOrDefaultAsync(v => v.Id == vokiId, cancellationToken: ct);
}