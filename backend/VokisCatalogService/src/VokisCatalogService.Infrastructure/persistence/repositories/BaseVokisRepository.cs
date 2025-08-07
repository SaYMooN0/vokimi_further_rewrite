using VokisCatalogService.Domain.common.interfaces.repositories;

namespace VokisCatalogService.Infrastructure.persistence.repositories;

internal class BaseVokisRepository : IBaseVokisRepository
{
    private readonly VokisCatalogTakingDbContext _db;

    public BaseVokisRepository(VokisCatalogTakingDbContext db) {
        _db = db;
    }


}