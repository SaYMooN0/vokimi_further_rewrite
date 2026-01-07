using SharedKernel;
using SharedKernel.domain.ids;
using SharedKernel.errs;

namespace ApplicationShared;

public interface IUserContext
{
 
    AppUserId AuthenticatedUserId { get; }
    AuthenticatedUserContext AuthenticatedUser { get; }
    ErrOr<AppUserId> UserIdFromToken();
}