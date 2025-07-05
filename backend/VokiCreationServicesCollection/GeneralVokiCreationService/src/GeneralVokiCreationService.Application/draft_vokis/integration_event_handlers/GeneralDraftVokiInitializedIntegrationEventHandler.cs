using GeneralVokiCreationService.Domain.draft_voki_aggregate;
using GeneralVokiCreationService.Domain.repositories;
using MassTransit;
using SharedKernel.common.vokis;
using SharedKernel.integration_events.draft_voki_initialized;

namespace GeneralVokiCreationService.Application.draft_vokis.integration_event_handlers;

public class GeneralDraftVokiInitializedIntegrationEventHandler : IConsumer<GeneralDraftVokiInitializedIntegrationEvent>
{
    private readonly IDraftVokiRepository _draftVokiRepository;

    public GeneralDraftVokiInitializedIntegrationEventHandler(IDraftVokiRepository draftVokiRepository) {
        _draftVokiRepository = draftVokiRepository;
    }

    public async Task Consume(ConsumeContext<GeneralDraftVokiInitializedIntegrationEvent> context) {
        DraftVoki newVoki = DraftVoki.Create(
            context.Message.VokiId,
            context.Message.PrimaryAuthorId,
            VokiName.Create(context.Message.VokiName).AsSuccess(),
            context.Message.CoverPath,
            context.Message.CreationDate
        );
        await _draftVokiRepository.Add(newVoki);
    }
}