using SharedKernel;
using SharedKernel.domain.ids;

namespace ApplicationShared;

public record AuthenticatedUserContext(
    AppUserId UserId
) : IAuthenticatedUserContext;