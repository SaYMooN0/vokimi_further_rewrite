﻿using MassTransit;
using SharedKernel.integration_events.voki_taken;
using VokiRatingsService.Application.common.repositories;
using VokiRatingsService.Domain.app_user_aggregate;

namespace VokiRatingsService.Application.app_users.integration_event_handlers;

public class BaseVokiTakenIntegrationEventHandler : IConsumer<BaseVokiTakenIntegrationEvent>
{
    private readonly IAppUsersRepository _appUsersRepository;

    public BaseVokiTakenIntegrationEventHandler(IAppUsersRepository appUsersRepository) {
        _appUsersRepository = appUsersRepository;
    }


    public async Task Consume(ConsumeContext<BaseVokiTakenIntegrationEvent> context) {
        if (context.Message.VokiTakerId is null) {
            return;
        }

        AppUser? appUser = await _appUsersRepository.GetById(context.Message.VokiTakerId);
        if (appUser is null) {
            return;
        }

        var changed = appUser.AddTakenVoki(context.Message.VokiId);
        if (changed) {
            await _appUsersRepository.Update(appUser);
        }
    }
}