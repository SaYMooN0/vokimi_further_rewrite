using GeneralVokiCreationService.Domain.draft_general_voki_aggregate;
using GeneralVokiCreationService.Domain.repositories;
using MassTransit;
using SharedKernel.common.vokis;
using SharedKernel.integration_events.draft_voki_initialized;

namespace GeneralVokiCreationService.Application.draft_vokis.integration_event_handlers;

public class GeneralDraftVokiInitializedIntegrationEventHandler : IConsumer<GeneralDraftVokiInitializedIntegrationEvent>
{
    private readonly IDraftGeneralVokiRepository _draftGeneralVokiRepository;

    public GeneralDraftVokiInitializedIntegrationEventHandler(IDraftGeneralVokiRepository draftGeneralVokiRepository) {
        _draftGeneralVokiRepository = draftGeneralVokiRepository;
    }

    public async Task Consume(ConsumeContext<GeneralDraftVokiInitializedIntegrationEvent> context) {
        DraftGeneralVoki newGeneralVoki = DraftGeneralVoki.Create(
            context.Message.VokiId,
            context.Message.PrimaryAuthorId,
            VokiName.Create(context.Message.VokiName).AsSuccess(),
            context.Message.CoverPath,
            context.Message.CreationDate
        );
        await _draftGeneralVokiRepository.Add(newGeneralVoki);
    }
}