using MassTransit;
using SharedKernel.integration_events.voki_publishing;
using VokiRatingsService.Application.common.repositories;
using VokiRatingsService.Domain.voki_aggregate;

namespace VokiRatingsService.Application.vokis.integration_event_handlers;

public class BaseVokiPublishedIntegrationEventHandler : IConsumer<BaseVokiPublishedIntegrationEvent>
{
    private readonly IVokisRepository _vokisRepository;
    public BaseVokiPublishedIntegrationEventHandler(IVokisRepository vokisRepository) {
        _vokisRepository = vokisRepository;
    }
    public async Task Consume(ConsumeContext<BaseVokiPublishedIntegrationEvent> context) {
        Voki voki = new Voki(context.Message.VokiId);
        await _vokisRepository.Add(voki);
    }
}