namespace SharedKernel.domain.events;

public interface IDomainEventHandler<in T> where T : IDomainEvent
{
    Task Handle(T e, CancellationToken cancellationToken);
}
