using MassTransit;
using SharedKernel.integration_events;
using VokisCatalogService.Application.common.repositories;
using VokisCatalogService.Domain.voki_aggregate;

namespace VokisCatalogService.Application.vokis.integration_event_handlers;

public class VokiRatedIntegrationEventHandler : IConsumer<VokiRatedIntegrationEvent>
{
    private readonly IBaseVokisRepository _baseVokisRepository;

    public VokiRatedIntegrationEventHandler(IBaseVokisRepository baseVokisRepository) {
        _baseVokisRepository = baseVokisRepository;
    }

    public async Task Consume(ConsumeContext<VokiRatedIntegrationEvent> context) {
        BaseVoki? voki = await _baseVokisRepository.GetById(context.Message.VokiId, context.CancellationToken);
        if (voki is null) {
            return;
        }

        voki.UpdateRatingsCount(context.Message.NewRatingsCount);
        await _baseVokisRepository.Update(voki, context.CancellationToken);
    }
}