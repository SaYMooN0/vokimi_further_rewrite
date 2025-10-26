using MassTransit;
using SharedKernel;
using SharedKernel.integration_events.voki_taken;
using VokisCatalogService.Application.common.repositories;
using VokisCatalogService.Domain.app_user_aggregate;
using VokisCatalogService.Domain.voki_aggregate;

namespace VokisCatalogService.Application.vokis.integration_event_handlers;

public class BaseVokiTakenIntegrationEventHandler : IConsumer<BaseVokiTakenIntegrationEvent>
{
    private readonly IBaseVokisRepository _baseVokisRepository;
    private readonly IAppUsersRepository _appUsersRepository;
    private readonly IDateTimeProvider _dateTimeProvider;

    public BaseVokiTakenIntegrationEventHandler(
        IBaseVokisRepository baseVokisRepository,
        IAppUsersRepository appUsersRepository,
        IDateTimeProvider dateTimeProvider
    ) {
        _baseVokisRepository = baseVokisRepository;
        _appUsersRepository = appUsersRepository;
        _dateTimeProvider = dateTimeProvider;
    }


    public async Task Consume(ConsumeContext<BaseVokiTakenIntegrationEvent> context) {
        BaseVoki? voki = await _baseVokisRepository.GetById(context.Message.VokiId, context.CancellationToken);
        if (voki is null) {
            return;
        }

        voki.UpdateVokiTakingsCount(context.Message.NewVokiTakingsCount);
        await _baseVokisRepository.Update(voki, context.CancellationToken);

        if (context.Message.VokiTakerId is null) {
            return;
        }

        AppUser? vokiTaker = await _appUsersRepository.GetUserWithTakenVokis(
            context.Message.VokiTakerId,
            context.CancellationToken
        );
        if (vokiTaker is null) {
            return;
        }

        vokiTaker.TakenVokis.Add(context.Message.VokiId, _dateTimeProvider.UtcNow);
        await _appUsersRepository.Update(vokiTaker, context.CancellationToken);
    }
}