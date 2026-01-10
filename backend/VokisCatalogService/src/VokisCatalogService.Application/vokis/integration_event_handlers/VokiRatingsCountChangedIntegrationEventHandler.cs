using MassTransit;
using SharedKernel.integration_events;
using VokisCatalogService.Application.common.repositories;
using VokisCatalogService.Domain.voki_aggregate;

namespace VokisCatalogService.Application.vokis.integration_event_handlers;

public class VokiRatingsCountChangedIntegrationEventHandler : IConsumer<VokiRatingsCountChangedIntegrationEvent>
{
    private readonly IBaseVokisRepository _baseVokisRepository;

    public VokiRatingsCountChangedIntegrationEventHandler(IBaseVokisRepository baseVokisRepository) {
        _baseVokisRepository = baseVokisRepository;
    }

    public async Task Consume(ConsumeContext<VokiRatingsCountChangedIntegrationEvent> context) {
        BaseVoki? voki = await _baseVokisRepository.GetByIdForUpdate(context.Message.VokiId, context.CancellationToken);
        if (voki is null) {
            return;
        }

        voki.UpdateRatingsCount(context.Message.NewRatingsCount);
        await _baseVokisRepository.Update(voki, context.CancellationToken);
    }
}