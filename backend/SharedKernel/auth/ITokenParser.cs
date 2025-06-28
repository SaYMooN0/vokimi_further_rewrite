namespace SharedKernel.auth;

public interface ITokenParser
{
    public ErrOr<AppUserId> UserIdFromJwtToken(JwtTokenString token);
}