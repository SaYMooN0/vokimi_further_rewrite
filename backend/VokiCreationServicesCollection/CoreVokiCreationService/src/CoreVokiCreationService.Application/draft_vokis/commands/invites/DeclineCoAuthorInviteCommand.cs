﻿using CoreVokiCreationService.Application.common.repositories;
using CoreVokiCreationService.Domain.app_user_aggregate;
using SharedKernel.auth;

namespace CoreVokiCreationService.Application.draft_vokis.commands.invites;

public sealed record DeclineCoAuthorInviteCommand(VokiId VokiId) :
    ICommand<ImmutableArray<VokiId>>;

internal sealed class DeclineCoAuthorInviteCommandHandler :
    ICommandHandler<DeclineCoAuthorInviteCommand, ImmutableArray<VokiId>>
{
    private readonly IAppUsersRepository _appUsersRepository;
    private readonly IUserContext _userContext;

    public DeclineCoAuthorInviteCommandHandler(IAppUsersRepository appUsersRepository, IUserContext userContext) {
        _appUsersRepository = appUsersRepository;
        _userContext = userContext;
    }


    public async Task<ErrOr<ImmutableArray<VokiId>>> Handle(DeclineCoAuthorInviteCommand command, CancellationToken ct) {
        AppUser user = (await _appUsersRepository.GetById(_userContext.AuthenticatedUserId))!;
        user.DeclineCoAuthorInvite(command.VokiId);
        await _appUsersRepository.Update(user);
        return user.InvitedToCoAuthorVokiIds.ToImmutableArray();
    }
}