﻿using CoreVokiCreationService.Application.common.repositories;
using CoreVokiCreationService.Domain.app_user_aggregate;
using CoreVokiCreationService.Domain.draft_voki_aggregate.events;

namespace CoreVokiCreationService.Application.app_users.domain_event_handlers;

internal class CoAuthorInviteCanceledEventHandler : IDomainEventHandler<CoAuthorInviteCanceledEvent>
{
    private readonly IAppUsersRepository _appUsersRepository;

    public CoAuthorInviteCanceledEventHandler(IAppUsersRepository appUsersRepository) {
        _appUsersRepository = appUsersRepository;
    }

    public async Task Handle(CoAuthorInviteCanceledEvent e, CancellationToken ct) {
        AppUser user = (await _appUsersRepository.GetById(e.AppUserId))!;
        user.RemoveInvitedToCoAuthorVoki(e.VokiId);
        await _appUsersRepository.Update(user);
    }
}