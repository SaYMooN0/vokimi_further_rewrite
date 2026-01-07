using SharedKernel.domain.events;

namespace InfrastructureShared.Base.domain_events_publisher;

public interface IDomainEventsPublisher
{
    Task Publish(IDomainEvent domainEvent, CancellationToken cancellationToken );
}
