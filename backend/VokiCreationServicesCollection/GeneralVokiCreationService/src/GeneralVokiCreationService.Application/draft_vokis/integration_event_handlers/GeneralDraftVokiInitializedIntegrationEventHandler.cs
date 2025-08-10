using GeneralVokiCreationService.Domain.common.interfaces.repositories;
using GeneralVokiCreationService.Domain.draft_general_voki_aggregate;
using MassTransit;
using SharedKernel.common.vokis;
using SharedKernel.integration_events.draft_vokis.new_voki_initialized;
using VokimiStorageKeysLib.voki_cover;

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
            new VokiName(  context.Message.VokiName),
            new VokiCoverKey(context.Message.Cover),
            context.Message.CreationDate
        );
        await _draftGeneralVokiRepository.Add(newGeneralVoki);
    }
}