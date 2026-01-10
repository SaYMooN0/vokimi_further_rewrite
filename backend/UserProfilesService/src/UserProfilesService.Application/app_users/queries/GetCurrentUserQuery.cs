using ApplicationShared.messaging.pipeline_behaviors;
using SharedKernel.user_ctx;
using UserProfilesService.Application.common.repositories;
using UserProfilesService.Domain.app_user_aggregate;

namespace UserProfilesService.Application.app_users.queries;

public sealed record GetCurrentUserQuery() :
    IQuery<AppUser>,
    IWithAuthCheckStep
{
    public Err UnauthenticatedErr => ErrFactory.AuthRequired("Could not get current user because user is not logged in");
}

internal sealed class GetCurrentUserQueryHandler : IQueryHandler<GetCurrentUserQuery, AppUser>
{
    private readonly IAppUsersRepository _appUsersRepository;
    private readonly IUserCtx _userCtx;

    public GetCurrentUserQueryHandler(
        IAppUsersRepository appUsersRepository,
        IUserCtx userCtx
    ) {
        _appUsersRepository = appUsersRepository;
        _userCtx = userCtx;
    }


    public async Task<ErrOr<AppUser>> Handle(GetCurrentUserQuery query, CancellationToken ct) {
        AppUser? user = await _appUsersRepository.GetCurrentUser(_userCtx.AuthenticatedUser, ct);

        if (user is null) {
            return ErrFactory.NotFound.User(
                "Current user was not found in the database",
                $"There is no users with id {_userCtx.AuthenticatedUser.UserId}"
            );
        }

        return user;
    }
}