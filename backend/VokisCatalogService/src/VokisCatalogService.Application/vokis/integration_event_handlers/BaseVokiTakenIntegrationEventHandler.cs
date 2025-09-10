using MassTransit;
using SharedKernel.integration_events.voki_taken;
using VokisCatalogService.Domain.app_user_aggregate;
using VokisCatalogService.Domain.common.interfaces.repositories;
using VokisCatalogService.Domain.voki_aggregate;

namespace VokisCatalogService.Application.vokis.integration_event_handlers;

public class BaseVokiTakenIntegrationEventHandler : IConsumer<BaseVokiTakenIntegrationEvent>
{
    private readonly IBaseVokisRepository _baseVokisRepository;
    private readonly IAppUsersRepository _appUsersRepository;

    public BaseVokiTakenIntegrationEventHandler(
        IBaseVokisRepository baseVokisRepository,
        IAppUsersRepository appUsersRepository
    ) {
        _baseVokisRepository = baseVokisRepository;
        _appUsersRepository = appUsersRepository;
    }


    public async Task Consume(ConsumeContext<BaseVokiTakenIntegrationEvent> context) {
        BaseVoki? voki = await _baseVokisRepository.GetById(context.Message.VokiId);
        if (voki is null) {
            return;
        }

        voki.UpdateVokiTakingsCount(context.Message.NewVokiTakingsCount);
        await _baseVokisRepository.Update(voki);

        if (context.Message.VokiTaker is not null) {
            AppUser? vokiTaker = await _appUsersRepository.GetById(context.Message.VokiTaker);
            if (vokiTaker is not null) {
                vokiTaker.AddTakenVoki(context.Message.VokiId);
                await _appUsersRepository.Update(vokiTaker);
            }
        }
    }
}