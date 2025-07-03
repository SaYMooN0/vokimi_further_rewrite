namespace SharedKernel.domain.events;

public abstract class BaseDomainToIntegrationEventsHandler;
public interface IDomainEventHandler<in T> where T : IDomainEvent
{
    Task Handle(T e, CancellationToken ct);
}
