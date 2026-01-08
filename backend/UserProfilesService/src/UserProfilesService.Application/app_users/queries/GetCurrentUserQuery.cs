using ApplicationShared;
using ApplicationShared.messaging.pipeline_behaviors;
using SharedKernel.user_ctx;
using UserProfilesService.Application.common.repositories;
using UserProfilesService.Domain.app_user_aggregate;

namespace UserProfilesService.Application.app_users.queries;

public sealed record GetCurrentUserQuery() :
    IQuery<AppUser>,
    IWithAuthCheckStep;

internal sealed class GetCurrentUserQueryHandler : IQueryHandler<GetCurrentUserQuery, AppUser>
{
    private readonly IAppUsersRepository _appUsersRepository;
    private readonly IUserContext _userContext;

    public GetCurrentUserQueryHandler(
        IAppUsersRepository appUsersRepository,
        IUserContext userContext
    ) {
        _appUsersRepository = appUsersRepository;
        _userContext = userContext;
    }


    public async Task<ErrOr<AppUser>> Handle(GetCurrentUserQuery query, CancellationToken ct) {
        ErrOr<AppUserId> userIdOrErr = _userContext.UserIdFromToken();
        if (userIdOrErr.IsErr()) {
            return ErrFactory.AuthRequired("Could not get current user because user is not logged in");
        }

        AppUserId userId = userIdOrErr.AsSuccess();

        AppUser? user = await _appUsersRepository.GetByIdAsNoTracking(userId, ct);
        if (user is null) {
            return ErrFactory.NotFound.User(
                "Current user was not found in the database",
                $"There is no users with id {userId}"
            );
        }

        return user;
    }
}