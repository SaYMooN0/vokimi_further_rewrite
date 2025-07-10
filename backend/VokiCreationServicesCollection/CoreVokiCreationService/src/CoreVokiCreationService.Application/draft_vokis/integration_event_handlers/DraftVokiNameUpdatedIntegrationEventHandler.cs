﻿using CoreVokiCreationService.Domain.common.interfaces.repositories;
using CoreVokiCreationService.Domain.draft_voki_aggregate;
using MassTransit;
using SharedKernel.common.vokis;
using SharedKernel.integration_events.draft_vokis;

namespace CoreVokiCreationService.Application.draft_vokis.integration_event_handlers;

public class DraftVokiNameUpdatedIntegrationEventHandler : IConsumer<DraftVokiNameUpdatedIntegrationEvent>
{
    private readonly IDraftVokiRepository _draftVokiRepository;

    public DraftVokiNameUpdatedIntegrationEventHandler(IDraftVokiRepository draftVokiRepository) {
        _draftVokiRepository = draftVokiRepository;
    }

    public async Task Consume(ConsumeContext<DraftVokiNameUpdatedIntegrationEvent> context) {
        DraftVoki voki = (await _draftVokiRepository.GetById(context.Message.VokiId))!;
        voki.UpdateName(new VokiName(context.Message.NewVokiName));
        await _draftVokiRepository.Update(voki);
    }
}