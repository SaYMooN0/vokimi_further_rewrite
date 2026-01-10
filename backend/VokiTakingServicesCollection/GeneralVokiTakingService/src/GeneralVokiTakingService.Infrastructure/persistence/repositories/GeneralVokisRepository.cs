using GeneralVokiTakingService.Application.common.repositories;
using GeneralVokiTakingService.Domain.general_voki_aggregate;
using InfrastructureShared.EfCore;
using InfrastructureShared.EfCore.query_extensions;
using Microsoft.EntityFrameworkCore;
using VokiTakingServicesLib.Domain.base_voki_aggregate;

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


    public Task<GeneralVoki?> GetWithQuestionAnswers(VokiId vokiId, CancellationToken ct) =>
        _db.Vokis
            .AsNoTracking()
            .Include(v => v.Questions)
            .ThenInclude(q => q.Answers)
            .FirstOrDefaultAsync(v => v.Id == vokiId, cancellationToken: ct);

    public Task<GeneralVoki?> GetWithResultsById(VokiId vokiId, CancellationToken ct) =>
        _db.Vokis
            .Include(v => EF.Property<List<VokiResult>>(v, "_results"))
            .FirstOrDefaultAsync(v => v.Id == vokiId, cancellationToken: ct);

    public async Task<GeneralVoki?> GetByIdForUpdate(VokiId vokiId, CancellationToken ct) =>
        await _db.Vokis
            .ForUpdate()
            .FirstOrDefaultAsync(v => v.Id == vokiId, cancellationToken: ct);

    public async Task Update(GeneralVoki voki, CancellationToken ct) {
        _db.ThrowIfDetached(voki);
        _db.Update(voki);
        await _db.SaveChangesAsync(ct);
    }

    public Task<GeneralVoki?> GetWithQuestionAnswersAndResults(VokiId vokiId, CancellationToken ct) =>
        _db.Vokis
            .Include(v => v.Questions)
            .ThenInclude(q => q.Answers)
            .AsSplitQuery()
            .Include(v => EF.Property<IReadOnlyCollection<VokiResult>>(v, "_results"))
            .FirstOrDefaultAsync(v => v.Id == vokiId, cancellationToken: ct);
}