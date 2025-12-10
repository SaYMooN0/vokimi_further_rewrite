using VokiRatingsService.Application.common.repositories;
using VokiRatingsService.Domain.voki_aggregate;

namespace VokiRatingsService.Infrastructure.persistence.repositories;

internal class VokisRepository : IVokisRepository
{
    private readonly VokiRatingsDbContext _db;

    public VokisRepository(VokiRatingsDbContext db) {
        _db = db;
    }

    public async Task<Voki?> GetById(VokiId vokiId, CancellationToken ct) =>
        await _db.Vokis.FindAsync([vokiId], cancellationToken: ct);

    public async Task Update(Voki voki, CancellationToken ct) {
        _db.Vokis.Update(voki);
        await _db.SaveChangesAsync(ct);
    }

    public async Task Add(Voki voki, CancellationToken ct) {
        await _db.Vokis.AddAsync(voki, ct);
        await _db.SaveChangesAsync(ct);
    }
}