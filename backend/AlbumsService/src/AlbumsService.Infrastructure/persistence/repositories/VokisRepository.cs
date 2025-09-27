using AlbumsService.Application.common.repositories;

namespace AlbumsService.Infrastructure.persistence.repositories;

internal class VokisRepository : IVokisRepository
{
    private readonly AlbumsDbContext _db;

    public VokisRepository(AlbumsDbContext db) {
        _db = db;
    }

}