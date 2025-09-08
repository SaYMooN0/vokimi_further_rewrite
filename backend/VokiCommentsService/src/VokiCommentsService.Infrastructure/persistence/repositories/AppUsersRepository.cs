using VokiCommentsService.Domain.common.interfaces.repositories;

namespace VokiCommentsService.Infrastructure.persistence.repositories;

internal class AppUsersRepository : IAppUsersRepository
{
    private readonly VokiCommentsDbContext _db;

    public AppUsersRepository(VokiCommentsDbContext db) {
        _db = db;
    }

}