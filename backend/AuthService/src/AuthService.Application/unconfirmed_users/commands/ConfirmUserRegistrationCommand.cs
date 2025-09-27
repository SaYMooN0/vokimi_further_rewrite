﻿using AuthService.Application.abstractions;
using AuthService.Application.common.repositories;
using AuthService.Domain.app_user_aggregate;
using AuthService.Domain.unconfirmed_user_aggregate;
using SharedKernel;
using SharedKernel.auth;

namespace AuthService.Application.unconfirmed_users.commands;

public sealed record ConfirmUserRegistrationCommand(UnconfirmedUserId UserId, string ConfirmationCode) : ICommand<JwtTokenString>;

internal sealed class ConfirmUserRegistrationCommandHandler : ICommandHandler<ConfirmUserRegistrationCommand, JwtTokenString>
{
    private readonly IAppUsersRepository _appUsersRepository;
    private readonly IUnconfirmedUsersRepository _unconfirmedUsersRepository;
    private readonly ITokenGenerator _tokenGenerator;
    private readonly IDateTimeProvider _dateTimeProvider;


    public ConfirmUserRegistrationCommandHandler(
        IAppUsersRepository appUsersRepository,
        IUnconfirmedUsersRepository unconfirmedUsersRepository,
        ITokenGenerator tokenGenerator,
        IDateTimeProvider dateTimeProvider
    ) {
        _appUsersRepository = appUsersRepository;
        _unconfirmedUsersRepository = unconfirmedUsersRepository;
        _tokenGenerator = tokenGenerator;
        _dateTimeProvider = dateTimeProvider;
    }

    public async Task<ErrOr<JwtTokenString>> Handle(ConfirmUserRegistrationCommand command, CancellationToken ct) {
        if (await _appUsersRepository.AnyUserWithId(new AppUserId(command.UserId.Value))) {
            return ErrFactory.Unspecified(
                "This user has already been confirmed",
                "Login into your account using email and password that you used when registering"
            );
        }

        UnconfirmedUser? unconfirmedUser = await _unconfirmedUsersRepository.GetById(command.UserId);
        if (unconfirmedUser is null) {
            return ErrFactory.NotFound.Common(
                "Couldn't find user to confirm. Maybe this user has already been confirmed or the link has expired"
            );
        }

        if (unconfirmedUser.TryConfirm(command.ConfirmationCode).IsErr(out var err)) {
            return err;
        }

        AppUser user = AppUser.CreateNew(
            unconfirmedUser.Id,
            unconfirmedUser.Email,
            unconfirmedUser.PasswordHash,
            unconfirmedUser.UserName,
            _dateTimeProvider
        );
        await _appUsersRepository.Add(user);
        await _unconfirmedUsersRepository.Delete(unconfirmedUser);

        return _tokenGenerator.CreateToken(user);
    }
}