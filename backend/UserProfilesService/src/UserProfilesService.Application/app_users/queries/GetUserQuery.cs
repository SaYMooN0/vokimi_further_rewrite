using UserProfilesService.Application.common.repositories;
using UserProfilesService.Domain.app_user_aggregate;

namespace UserProfilesService.Application.app_users.queries;

public sealed record GetUserQuery(
    AppUserId UserId
) : IQuery<AppUser>;

internal sealed class GetUserQueryHandler : IQueryHandler<GetUserQuery, AppUser>
{
    private readonly IAppUsersRepository _appUsersRepository;

    public GetUserQueryHandler(IAppUsersRepository appUsersRepository) {
        _appUsersRepository = appUsersRepository;
    }


    public async Task<ErrOr<AppUser>> Handle(GetUserQuery query, CancellationToken ct) {
        AppUser? user = await _appUsersRepository.GetByIdAsNoTracking(query.UserId, ct);
        if (user is null) {
            return ErrFactory.NotFound.User(
                "Requested user was not found",
                $"There is no users with id {query.UserId}"
            );
        }

        return user;
    }
}