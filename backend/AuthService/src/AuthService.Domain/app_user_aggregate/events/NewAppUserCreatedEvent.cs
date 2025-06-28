using SharedKernel.domain.events;
using SharedKernel.domain.ids;

namespace AuthService.Domain.app_user_aggregate.events;

public record NewAppUserCreatedEvent(
    AppUserId CreatedUserId
) : IDomainEvent;