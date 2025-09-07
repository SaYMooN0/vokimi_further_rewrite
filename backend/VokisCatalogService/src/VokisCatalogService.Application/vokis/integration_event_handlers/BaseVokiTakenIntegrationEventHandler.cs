using MassTransit;
using SharedKernel.integration_events.voki_taken;
using VokisCatalogService.Domain.common.interfaces.repositories;
using VokisCatalogService.Domain.voki_aggregate;

namespace VokisCatalogService.Application.vokis.integration_event_handlers;

public class BaseVokiTakenIntegrationEventHandler : IConsumer<BaseVokiTakenIntegrationEvent>
{
    private readonly IBaseVokisRepository _baseVokisRepository;
    public BaseVokiTakenIntegrationEventHandler(IBaseVokisRepository baseVokisRepository) {
        _baseVokisRepository = baseVokisRepository;
    }


    public async Task Consume(ConsumeContext<BaseVokiTakenIntegrationEvent> context) {
        BaseVoki? voki = await _baseVokisRepository.GetById(context.Message.VokiId);
        if (voki is null) {
            return;
        }

        voki.UpdateVokiTakingsCount(context.Message.NewVokiTakingsCount);
        await _baseVokisRepository.Update(voki);    
    }
}