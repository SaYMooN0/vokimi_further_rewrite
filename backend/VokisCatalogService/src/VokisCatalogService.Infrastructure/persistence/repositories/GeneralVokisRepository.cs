using VokisCatalogService.Application.common.repositories;
using VokisCatalogService.Domain.voki_aggregate.voki_types;

namespace VokisCatalogService.Infrastructure.persistence.repositories;

internal class GeneralVokisRepository : IGeneralVokisRepository
{
    private readonly VokisCatalogDbContext _db;

    public GeneralVokisRepository(VokisCatalogDbContext db) {
        _db = db;
    }

    public async Task Add(GeneralVoki voki, CancellationToken ct) {
        await _db.GeneralVokis.AddAsync(voki, ct);
        await _db.SaveChangesAsync(ct);
    }
}