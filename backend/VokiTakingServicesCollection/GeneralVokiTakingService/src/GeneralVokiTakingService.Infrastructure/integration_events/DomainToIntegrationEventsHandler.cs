﻿namespace GeneralVokiTakingService.Infrastructure.integration_events;

internal class DomainToIntegrationEventsHandler : IDomainToIntegrationEventsHandler

// and all other domain events that need to be published as integration events
{
    private readonly IIntegrationEventPublisher _integrationEventPublisher;

    public DomainToIntegrationEventsHandler(IIntegrationEventPublisher integrationEventPublisher) {
        _integrationEventPublisher = integrationEventPublisher;
    }

}