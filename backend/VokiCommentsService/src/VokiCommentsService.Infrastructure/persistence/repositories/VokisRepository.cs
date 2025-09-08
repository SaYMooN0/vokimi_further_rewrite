using VokiCommentsService.Domain.common.interfaces.repositories;

namespace VokiCommentsService.Infrastructure.persistence.repositories;

internal class VokisRepository : IVokisRepository
{
    private readonly VokiCommentsDbContext _db;

    public VokisRepository(VokiCommentsDbContext db) {
        _db = db;
    }

}