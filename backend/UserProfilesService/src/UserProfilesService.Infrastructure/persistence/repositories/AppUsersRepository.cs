using Microsoft.EntityFrameworkCore;
using UserProfilesService.Application.common.repositories;
using UserProfilesService.Domain.app_user_aggregate;

namespace UserProfilesService.Infrastructure.persistence.repositories;

public class AppUsersRepository : IAppUsersRepository
{
    private readonly UserProfilesDbContext _db;

    public AppUsersRepository(UserProfilesDbContext db) {
        _db = db;
    }

    public async Task Add(AppUser user, CancellationToken ct) {
        await _db.AppUsers.AddAsync(user, ct);
        await _db.SaveChangesAsync(ct);
    }

    public async Task<AppUser?> GetById(AppUserId id, CancellationToken ct) =>
        await _db.AppUsers.FindAsync([id], cancellationToken: ct);

    public async Task Update(AppUser user, CancellationToken ct) {
        _db.Update(user);
        await _db.SaveChangesAsync(ct);
    }

    public Task<AppUser?> GetByIdAsNoTracking(AppUserId userId, CancellationToken ct) => _db.AppUsers
        .AsNoTracking()
        .FirstOrDefaultAsync(x => x.Id == userId, ct);

    public Task<UserPreviewDto[]> GetUserNamesWithProfilePics(
        IEnumerable<AppUserId> userIds,
        CancellationToken ct
    ) {
        AppUserId[] userIdsArray = userIds as AppUserId[] ?? userIds.ToArray();

        if (!userIdsArray.Any()) {
            return Task.FromResult(Array.Empty<UserPreviewDto>());
        }

        return _db.AppUsers
            .AsNoTracking()
            .Where(u => userIdsArray.Contains(u.Id))
            .Select(u => new UserPreviewDto(u.Id, u.UniqueName, u.DisplayName, u.ProfilePic))
            .ToArrayAsync(ct);
    }

  

    public async Task<UserPreviewWithAllowInvitesSettingDto[]> SearchToInviteByNameQuery(
        string searchValue, int limit, CancellationToken ct) {
        int queryLimit = Math.Min(limit, 100);
        if (string.IsNullOrWhiteSpace(searchValue)) {
            return [];
        }

        string normalized = searchValue.Trim().ToLowerInvariant();
        bool startsWithAt = normalized.StartsWith('@');
        string searchWithoutAt = startsWithAt ? normalized[1..] : normalized;

        string pattern = $"%{searchWithoutAt}%";

        string exactDisplayLike = $"{normalized} %"; // display_name + space
        string exactUniqueLike = $"% {searchWithoutAt}"; // space + unique_name

        var query = _db.AppUsers
            .AsNoTracking()
            .Where(u =>
                EF.Functions.ILike(EF.Property<string>(u, "SearchableName"), pattern)
            )
            .Select(u => new {
                User = u,
                ExactMatch =
                    EF.Functions.ILike(EF.Property<string>(u, "SearchableName"), exactDisplayLike) ||
                    EF.Functions.ILike(EF.Property<string>(u, "SearchableName"), exactUniqueLike)
            })
            .OrderByDescending(x => x.ExactMatch)
            .ThenBy(x => x.User.DisplayName)
            .Take(queryLimit)
            .Select(x => new UserPreviewWithAllowInvitesSettingDto(
                x.User.Id,
                x.User.UniqueName,
                x.User.DisplayName,
                x.User.ProfilePic,
                x.User.Settings.AllowCoAuthorInvites
            ));

        return await query.ToArrayAsync(ct);
    }
}