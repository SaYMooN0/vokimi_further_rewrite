using VokisCatalogService.Domain.common.interfaces.repositories;
using VokisCatalogService.Domain.voki_aggregate.voki_types;

namespace VokisCatalogService.Infrastructure.persistence.repositories;

public class GeneralVokisRepository : IGeneralVokisRepository
{
    private readonly VokisCatalogDbContext _db;

    public GeneralVokisRepository(VokisCatalogDbContext db) {
        _db = db;
    }

    public async Task Add(GeneralVoki voki) {
        await _db.GeneralVokis.AddAsync(voki);
        await _db.SaveChangesAsync();
    }
}