namespace SharedKernel.user_ctx;

public class AuthenticatedUserCtx : IUserCtx
{
    public AppUserId UserId { get; private set; }

    internal AuthenticatedUserCtx(AppUserId userId) {
        UserId = userId;
    }

    public bool IsAuthenticated => true;
    public ErrOr<AppUserId> TryGetUserId => UserId;
}