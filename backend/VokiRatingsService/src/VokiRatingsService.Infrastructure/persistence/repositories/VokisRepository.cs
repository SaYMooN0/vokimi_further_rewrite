using VokiRatingsService.Application.common.repositories;

namespace VokiRatingsService.Infrastructure.persistence.repositories;

internal class VokisRepository : IVokisRepository
{
    private readonly VokiRatingsDbContext _db;

    public VokisRepository(VokiRatingsDbContext db) {
        _db = db;
    }

}