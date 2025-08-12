using GeneralVokiCreationService.Domain.common.interfaces.repositories;
using GeneralVokiCreationService.Domain.draft_general_voki_aggregate.events;
using Microsoft.Extensions.Logging;

namespace GeneralVokiCreationService.Application.draft_vokis.domain_event_handlers;

internal class GeneralVokiPublishedEventHandler : IDomainEventHandler<GeneralVokiPublishedEvent>
{
    private readonly IDraftGeneralVokiRepository _draftGeneralVokiRepository;
    private readonly ILogger<GeneralVokiPublishedEventHandler> _logger;

    public GeneralVokiPublishedEventHandler(
        IDraftGeneralVokiRepository draftGeneralVokiRepository,
        ILogger<GeneralVokiPublishedEventHandler> logger
    ) {
        _draftGeneralVokiRepository = draftGeneralVokiRepository;
        _logger = logger;
    }

    public async Task Handle(GeneralVokiPublishedEvent e, CancellationToken ct) {
        var voki = await _draftGeneralVokiRepository.GetById(e.VokiId);
        if (voki is not null) {
            await _draftGeneralVokiRepository.Delete(voki);
        }
        else {
            _logger.LogError(
                "Could not delete draft general voki that has benn published, {vokiId}, {name}",
                e.VokiId, e.Name
            );
        }
    }
}