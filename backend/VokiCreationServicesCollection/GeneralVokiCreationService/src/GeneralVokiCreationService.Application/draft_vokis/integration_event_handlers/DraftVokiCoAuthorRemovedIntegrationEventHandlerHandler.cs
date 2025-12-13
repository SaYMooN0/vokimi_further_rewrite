using GeneralVokiCreationService.Application.common;
using GeneralVokiCreationService.Domain.draft_general_voki_aggregate;
using MassTransit;
using Microsoft.Extensions.Logging;
using SharedKernel.common.vokis;
using SharedKernel.integration_events.draft_vokis.co_authors;

namespace GeneralVokiCreationService.Application.draft_vokis.integration_event_handlers;

public class
    DraftVokiCoAuthorRemovedIntegrationEventHandlerHandler : IConsumer<DraftVokiCoAuthorRemovedIntegrationEvent>
{
    private readonly IDraftGeneralVokisRepository _draftGeneralVokisRepository;
    private readonly ILogger<DraftVokiCoAuthorRemovedIntegrationEventHandlerHandler> _logger;

    public DraftVokiCoAuthorRemovedIntegrationEventHandlerHandler(
        IDraftGeneralVokisRepository draftGeneralVokisRepository,
        ILogger<DraftVokiCoAuthorRemovedIntegrationEventHandlerHandler> logger
    ) {
        _draftGeneralVokisRepository = draftGeneralVokisRepository;
        _logger = logger;
    }

    public async Task Consume(ConsumeContext<DraftVokiCoAuthorRemovedIntegrationEvent> context) {
        var msg = context.Message;
        var eventName = nameof(DraftVokiCoAuthorRemovedIntegrationEvent);

        if (msg.VokiType != VokiType.General) {
            _logger.LogInformation(
                "Skipping {EventName} for Voki {VokiId}: unsupported Voki type {VokiType}",
                eventName, msg.VokiId, msg.VokiType
            );
            return;
        }

        DraftGeneralVoki? voki = await _draftGeneralVokisRepository.GetById(msg.VokiId, context.CancellationToken);
        if (voki is null) {
            _logger.LogWarning(
                "Received {EventName} but draft voki {VokiId} was not found. Could not remove co-author {AppUserId}",
                eventName, msg.VokiId, msg.AppUserId
            );
            return;
        }

        ErrOrNothing result = voki.RemoveCoAuthor(
            msg.AppUserId,
            msg.UserIdsExpectedToBecomeManagers.ToImmutableHashSet()
        );

        if (result.IsErr(out var err)) {
            _logger.LogError(
                "Failed to handle {EventName} for Voki {VokiId}. Could not remove co-author {AppUserId}. Err: {Err}",
                eventName, msg.VokiId, msg.AppUserId, err.ToString()
            );

            UnexpectedBehaviourException.ThrowErr(err);
        }

        await _draftGeneralVokisRepository.Update(voki, context.CancellationToken);

        _logger.LogInformation(
            "Processed {EventName}: co-author {AppUserId} removed from Voki {VokiId}",
            eventName, msg.AppUserId, msg.VokiId
        );
    }
}