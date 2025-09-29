using ApplicationShared;
using SharedKernel.integration_events;
using VokiRatingsService.Domain.voki_aggregate.events;

namespace VokiRatingsService.Application;

internal class DomainToIntegrationEventsHandler : IDomainToIntegrationEventsHandler,
    IDomainEventHandler<NewRatingToVokiAddedEvent>
// and all other domain events that need to be published as integration events
{
    private readonly IIntegrationEventPublisher _integrationEventPublisher;

    public DomainToIntegrationEventsHandler(IIntegrationEventPublisher integrationEventPublisher) {
        _integrationEventPublisher = integrationEventPublisher;
    }


    public async Task Handle(NewRatingToVokiAddedEvent e, CancellationToken ct) =>
        await _integrationEventPublisher.Publish(new VokiRatedIntegrationEvent(
            e.VokiId, e.NewRatingsCount
        ), ct);
}