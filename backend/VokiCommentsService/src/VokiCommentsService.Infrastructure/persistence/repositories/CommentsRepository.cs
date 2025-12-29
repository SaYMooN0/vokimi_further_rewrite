using VokiCommentsService.Application.common.repositories;

namespace VokiCommentsService.Infrastructure.persistence.repositories;

internal class CommentsRepository : ICommentsRepository
{
    private readonly VokiCommentsDbContext _db;

    public CommentsRepository(VokiCommentsDbContext db) {
        _db = db;
    }

    public Task<VokiIdWithLastCommentedDateDto[]> OrderedIdsOfVokiCommentedByUser(
        AppUserId userId, CancellationToken ct
    ) => Task.FromResult<VokiIdWithLastCommentedDateDto[]>([]);
    // _db.Comments
    // .AsNoTracking()
    // .Where(r => r.UserId == userId)
    // .Select(r => new VokiIdWithLastCommentedDateDto(r.VokiId, r.DateTime))
    // .OrderByDescending(r => r.Date)
    // .ToArrayAsync(ct);
}