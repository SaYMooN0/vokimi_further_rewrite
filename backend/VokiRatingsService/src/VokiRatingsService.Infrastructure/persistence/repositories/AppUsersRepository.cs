using VokiRatingsService.Domain.common.interfaces.repositories;

namespace VokiRatingsService.Infrastructure.persistence.repositories;

internal class AppUsersRepository : IAppUsersRepository
{
    private readonly VokiRatingsDbContext _db;

    public AppUsersRepository(VokiRatingsDbContext db) {
        _db = db;
    }

}