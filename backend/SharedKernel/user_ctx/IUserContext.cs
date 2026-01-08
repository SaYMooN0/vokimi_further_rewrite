namespace SharedKernel.user_ctx;

public interface IUserContext
{
 
    AppUserId AuthenticatedUserId { get; }
    AuthenticatedUserCtx AuthenticatedUser { get; }
    ErrOr<AppUserId> UserIdFromToken();
}