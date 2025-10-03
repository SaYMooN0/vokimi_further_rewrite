using SharedKernel.common.app_users;
using UserProfilesService.Application.common.repositories;
using UserProfilesService.Domain.app_user_aggregate;
using VokimiStorageKeysLib.concrete_keys;

namespace UserProfilesService.Application.app_users.queries;

public sealed record ListUsersNamesWithProfilePicsQuery(ImmutableHashSet<AppUserId> UserIds) :
    IQuery<Dictionary<AppUserId, (AppUserName Name, UserProfilePicKey PicKey)>>;

internal sealed class ListUsersNamesWithProfilePicsQueryHandler : IQueryHandler<ListUsersNamesWithProfilePicsQuery,
    Dictionary<AppUserId, (AppUserName Name, UserProfilePicKey PicKey)>>
{
    private readonly IAppUsersRepository _appUsersRepository;

    public ListUsersNamesWithProfilePicsQueryHandler(IAppUsersRepository appUsersRepository) {
        _appUsersRepository = appUsersRepository;
    }


    public async Task<ErrOr<Dictionary<AppUserId, (AppUserName Name, UserProfilePicKey PicKey)>>> Handle(
        ListUsersNamesWithProfilePicsQuery query, CancellationToken ct
    ) =>
        await _appUsersRepository.GetUserNamesWithProfilePics(query.UserIds, ct);
}