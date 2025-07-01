namespace AuthService.Domain.app_user_aggregate.events;

public record NewAppUserCreatedEvent(
    AppUserId CreatedUserId,
    AppUserName UserName,
    DateTime RegistrationDate
) : IDomainEvent;