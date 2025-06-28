using AuthService.Domain.app_user_aggregate;
using SharedKernel.auth;

namespace AuthService.Application.abstractions;

public interface ITokenGenerator
{
    public JwtTokenString CreateToken(AppUser user);
}