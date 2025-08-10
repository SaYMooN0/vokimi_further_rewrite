using InfrastructureShared.domain_events_publisher;
using SharedKernel.domain.events;

namespace DbSeeder;

internal class FakePublisher : IDomainEventsPublisher
{
    private FakePublisher() { }
    public Task Publish(IDomainEvent domainEvent, CancellationToken cancellationToken = default) => Task.CompletedTask;
    public static readonly FakePublisher Instance = new ();
}