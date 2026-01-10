namespace SharedKernel.user_ctx;

public class UnauthenticatedUserCtx : IUserCtx
{
    public UnauthenticatedUserCtx(Err? authErr =null ) {
        AuthErr = authErr ?? ErrFactory.AuthRequired("User is not authenticated");
    }

    public bool IsAuthenticated => false;
    public ErrOr<AppUserId> TryGetUserId => AuthErr;
    public Err AuthErr { get; }
}