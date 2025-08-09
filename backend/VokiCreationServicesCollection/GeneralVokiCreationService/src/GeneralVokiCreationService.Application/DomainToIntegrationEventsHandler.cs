﻿using ApplicationShared;
using GeneralVokiCreationService.Application.domain_to_integration_event_mappers;
using GeneralVokiCreationService.Domain.draft_general_voki_aggregate.events;
using SharedKernel.integration_events.draft_vokis;
using SharedKernel.integration_events.voki_publishing;
using VokiCreationServicesLib.Domain.draft_voki_aggregate.events;

namespace GeneralVokiCreationService.Application;

internal class DomainToIntegrationEventsHandler : IDomainToIntegrationEventsHandler,
    IDomainEventHandler<VokiNameUpdatedEvent>,
    IDomainEventHandler<VokiCoverUpdatedEvent>,
    IDomainEventHandler<GeneralVokiPublishedEvent>

// and all other domain events that need to be published as integration events
{
    private readonly IIntegrationEventPublisher _integrationEventPublisher;

    public DomainToIntegrationEventsHandler(IIntegrationEventPublisher integrationEventPublisher) {
        _integrationEventPublisher = integrationEventPublisher;
    }

    public async Task Handle(VokiNameUpdatedEvent e, CancellationToken ct) =>
        await _integrationEventPublisher.Publish(new DraftVokiNameUpdatedIntegrationEvent(
            e.VokiId, e.NewName.ToString()
        ), ct);

    public async Task Handle(VokiCoverUpdatedEvent e, CancellationToken ct) =>
        await _integrationEventPublisher.Publish(new DraftVokiCoverUpdatedIntegrationEvent(
            e.VokiId, e.NewCover.ToString()
        ), ct);

    public async Task Handle(GeneralVokiPublishedEvent e, CancellationToken ct) =>
        await _integrationEventPublisher.Publish(new GeneralVokiPublishedIntegrationEvent(
            e.VokiId,
            e.PrimaryAuthorId,
            CoAuthors: e.CoAuthors.ToArray(),
            Name: e.Name.ToString(),
            Cover: e.Cover.ToString(),
            Description: e.Details.Description.ToString(),
            IsAgeRestricted: e.Details.IsAgeRestricted,
            Language: e.Details.Language,
            Tags: e.Tags.Value.ToArray(),
            InitializingDate: e.InitializingDate,
            PublishingDate: e.PublishingDate,
            VokiPublishedEventMapper.QuestionIntegrationEventDtoArray(e.Questions),
            ForceSequentialAnswering: e.ForceSequentialAnswering,
            ShuffleQuestions: e.ShuffleQuestions,
            VokiPublishedEventMapper.ResultIntegrationEventDtoArray(e.Results)

        ), ct);
}